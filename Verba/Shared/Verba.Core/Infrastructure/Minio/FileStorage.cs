using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Verba.Abstractions.Application.Settings;
using Verba.Abstractions.FileStorage;

namespace Verba.Core.Infrastructure.Minio;

public class FileStorage : IFileStorage
{
    private readonly MinioClient _minioClient;
    private readonly ILogger<FileStorage> _logger;

    public FileStorage(IOptions<MinioSettings> options, ILogger<FileStorage> logger, IHostEnvironment env)
    {
        var endpoint = options.Value ?? throw new ArgumentNullException(nameof(options));
        _minioClient = (MinioClient)new MinioClient()
            .WithEndpoint(endpoint.MinioEndpoint)
            .WithCredentials(endpoint.MinioAccessKey, endpoint.MinioSecretKey)
            .Build();

        if (env.IsProduction())
        {
            _minioClient = (MinioClient)_minioClient.WithSSL(true);
        }

        _logger = logger;
    }

    public async Task<ObjectStat> GetObjectInfoAsync(string bucketName, Guid objectName)
    {
        try
        {
            var beArgs = new StatObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName.ToString());
            return await _minioClient.StatObjectAsync(beArgs);
        }
        catch (MinioException error)
        {
            _logger.LogError(error, error.Message);
            return null;
        }
    }

    public async Task RemoveObjectAsync(string bucketName, Guid objectName)
    {
        try
        {
            var beArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName.ToString());
            await _minioClient.RemoveObjectAsync(beArgs);
        }
        catch (MinioException error)
        {
            _logger.LogError(error, error.Message);
            throw;
        }
    }

    public async Task RemoveBucketAsync(string bucketName)
    {
        try
        {
            var beArgs = new RemoveBucketArgs()
                .WithBucket(bucketName);
            await _minioClient.RemoveBucketAsync(beArgs);
        }
        catch (MinioException error)
        {
            _logger.LogError(error, error.Message);
            throw;
        }
    }

    public async Task UploadAsync(string bucketName, Guid objectName, IFormFile file)
    {
        try
        {
            var beArgs = new BucketExistsArgs()
                .WithBucket(bucketName);
            bool found = await _minioClient.BucketExistsAsync(beArgs);
            if (!found)
            {
                try
                {
                    var beArgsMake = new MakeBucketArgs()
                        .WithBucket(bucketName);
                    await _minioClient.MakeBucketAsync(beArgsMake);
                }
                catch (MinioException e)
                {
                    if (e.Message != "BucketAlreadyOwnedByYou")
                        throw;
                }
            }

            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                ms.Position = 0;

                var beArgsPut = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName.ToString())
                    .WithStreamData(ms)
                    .WithObjectSize(ms.Length)
                    .WithContentType(file.ContentType);

                await _minioClient.PutObjectAsync(beArgsPut);
            }
        }
        catch (MinioException error)
        {
            _logger.LogError(error, error.Message);
            throw;
        }
    }

    public async Task<MemoryStream> DownloadAsync(string bucketName, Guid objectName)
    {
        try
        {
            var ms = new MemoryStream();
            var beArgs = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName.ToString())
                .WithCallbackStream((stream) =>
                {
                    stream.CopyTo(ms);
                });

            await _minioClient.GetObjectAsync(beArgs);

            ms.Position = 0;

            return ms;
        }
        catch (MinioException error)
        {
            _logger.LogError(error, error.Message);
            throw;
        }
    }

    public async Task CopyObjectAsync(string bucketName, Guid objectName, string destBucketName, Guid destObjectName)
    {
        try
        {
            var beArgsEx = new BucketExistsArgs().WithBucket(destBucketName);
            if (!await _minioClient.BucketExistsAsync(beArgsEx))
            {
                var beArgsMake = new MakeBucketArgs().WithBucket(destBucketName);
                await _minioClient.MakeBucketAsync(beArgsMake);
            }

            var beArgsCopySource = new CopySourceObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName.ToString());

            var beArgsCopyRequest = new CopyObjectArgs()
                .WithBucket(destBucketName)
                .WithObject(destObjectName.ToString())
                .WithCopyObjectSource(beArgsCopySource);

            await _minioClient.CopyObjectAsync(beArgsCopyRequest);
        }
        catch (MinioException error)
        {
            _logger.LogError(error, error.Message);
            throw;
        }
    }
}