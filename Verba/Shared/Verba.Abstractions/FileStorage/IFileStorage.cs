using System;
using System.IO;
using System.Threading.Tasks;
using Minio.DataModel;
using Microsoft.AspNetCore.Http;

namespace Verba.Abstractions.FileStorage
{
    public interface IFileStorage
    {
        Task<ObjectStat> GetObjectInfoAsync(string bucketName, Guid objectName);

        Task RemoveObjectAsync(string bucketName, Guid objectName);

        Task RemoveBucketAsync(string bucketName);

        Task UploadAsync(string bucketName, Guid objectName, IFormFile file);

        Task<MemoryStream> DownloadAsync(string bucketName, Guid objectName);

        Task CopyObjectAsync(string bucketName, Guid objectName, string destBucketName, Guid destObjectName);
    }
}