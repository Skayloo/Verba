using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using Verba.Abstractions.FileStorage;
using Verba.Core.Application.Authorization;
using Verba.Stock.Domain.ModelsForElastic.Entities;
using Verba.Stock.Core.BackgroundWorkers;
using Elasticsearch.Net;

namespace Verba.Stock.Core.Controllers;


[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/files")]
[ApiController]
public class FilesOperations : ControllerBase
{
    private readonly IElasticClient _elasticClient;
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<FilesOperations> _logger;
    private readonly IDocxFormatierWorker _documentFormatter;

    public FilesOperations(IElasticClient elasticClient, ILogger<FilesOperations> logger, IFileStorage fileStorage, IDocxFormatierWorker documentFormatter)
    {
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        _elasticClient = elasticClient;
        _logger = logger;
        _documentFormatter = documentFormatter;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Any)]
    [Route("get_entity_photo")]
    public async Task<IActionResult> GetEntityPhoto(string guid)
    {
        var result = await _elasticClient.SearchAsync<Entity>(sa => sa
            .Size(1)
            .PostFilter(pf => pf
                .Bool(b => b.
                    Must(must => must
                    .MatchPhrase(m => m
                        .Field(f => f.Guid)
                        .Query(guid))))));

        var modifiedResult = result.Documents.FirstOrDefault(x => x.Guid == guid);

        if (modifiedResult == null)
            return BadRequest();

        var doc = JsonConvert.DeserializeObject<Entity>(JsonConvert.SerializeObject(modifiedResult));

        if (doc.Avatar == null)
            return BadRequest("Nothing to show");

        var stat = await _fileStorage.GetObjectInfoAsync(doc.Avatar.FileBucket, new Guid(doc.Avatar.Guid));
        if (stat == null)
            return BadRequest("Unable to find requested photo");

        var ms = await _fileStorage.DownloadAsync(doc.Avatar.FileBucket, new Guid(doc.Avatar.Guid));

        return new FileStreamResult(ms, "application/octet-stream") { FileDownloadName = doc.Avatar.Filename };
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Any)]
    [Route("get_entity_file")]
    public async Task<IActionResult> GetEntityFile(string guid, string fileGuid)
    {
        var result = await _elasticClient.SearchAsync<Entity>(sa => sa
            .Size(1)
            .PostFilter(pf => pf
                .Bool(b => b.
                    Must(must => must
                    .MatchPhrase(m => m
                        .Field(f => f.Guid)
                        .Query(guid))))));

        var modifiedResult = result.Documents.FirstOrDefault(x => x.Guid == guid);

        if (modifiedResult == null)
            return BadRequest();

        var doc = JsonConvert.DeserializeObject<Entity>(JsonConvert.SerializeObject(modifiedResult));

        var fileToSend = doc.File.FirstOrDefault(x => x.Guid == fileGuid);

        var stat = await _fileStorage.GetObjectInfoAsync(fileToSend.FileBucket, new Guid(fileToSend.Guid));
        if (stat == null)
            return BadRequest("Unable to find requested file");

        var ms = await _fileStorage.DownloadAsync(fileToSend.FileBucket, new Guid(fileToSend.Guid));

        return new FileStreamResult(ms, "application/octet-stream") { FileDownloadName = fileToSend.Filename };
    }

    [HttpPatch]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Uploader)]
    [Route("add_entity_photo")]
    public async Task<IActionResult> PatchEntityPhoto(string guid, IFormFile avatar)
    {
        var result = await _elasticClient.SearchAsync<Entity>(sa => sa
                    .Size(1)
                    .PostFilter(pf => pf
                        .Bool(b => b.
                            Must(must => must
                            .MatchPhrase(m => m
                                .Field(f => f.Guid)
                                .Query(guid))))));

        var modifiedResult = result.Documents.FirstOrDefault(x => x.Guid == guid);

        var newDoc = JsonConvert.DeserializeObject<Entity>(JsonConvert.SerializeObject(modifiedResult));

        var newGuidFile = Guid.NewGuid();

        newDoc.Avatar = new Domain.ModelsForElastic.Entities.Avatar.Avatar
        {
            Guid = newGuidFile.ToString(),
            Filename = avatar.FileName,
            FileBucket = newDoc.Guid
        };

        await _fileStorage.UploadAsync(newDoc.Guid, newGuidFile, avatar);

        var stat = await _fileStorage.GetObjectInfoAsync(newDoc.Guid, newGuidFile);

        if (stat == null)
            return BadRequest();

        await _elasticClient.IndexAsync(newDoc, idx => idx
            .Index("entity")
            .Id(newDoc.Guid)
            .Refresh(Refresh.WaitFor));

        return Ok();
    }

    [HttpPatch]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Uploader)]
    [Route("add_entity_file")]
    public async Task<IActionResult> PatchEntityFile(string guid, IFormFile file)
    {
        var result = await _elasticClient.SearchAsync<Entity>(sa => sa
                    .Size(1)
                    .PostFilter(pf => pf
                        .Bool(b => b.
                            Must(must => must
                            .MatchPhrase(m => m
                                .Field(f => f.Guid)
                                .Query(guid))))));

        var modifiedResult = result.Documents.FirstOrDefault(x => x.Guid == guid);

        if (modifiedResult == null)
            return BadRequest();

        var newDoc = JsonConvert.DeserializeObject<Entity>(JsonConvert.SerializeObject(modifiedResult));

        var newGuidFile = Guid.NewGuid();

        newDoc.File.Add(new Domain.ModelsForElastic.Entities.Files.File
        {
            Guid = newGuidFile.ToString(),
            Filename = file.FileName,
            FileBucket = newDoc.Guid
        });

        await _elasticClient.IndexAsync(newDoc, idx => idx
            .Index("entity")
            .Id(newDoc.Guid)
            .Refresh(Refresh.WaitFor));

        await _fileStorage.UploadAsync(newDoc.Guid, newGuidFile, file);

        var stat = await _fileStorage.GetObjectInfoAsync(newDoc.Guid, newGuidFile);

        if (stat == null)
            return BadRequest();

        return Ok();
    }

    [HttpDelete]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Uploader)]
    [Route("delete_entity_file")]
    public async Task<IActionResult> DeleteEntityFile(string guid, string fileGuid)
    {
        var result = await _elasticClient.SearchAsync<Entity>(sa => sa
                    .Size(1)
                    .PostFilter(pf => pf
                        .Bool(b => b.
                            Must(must => must
                            .MatchPhrase(m => m
                                .Field(f => f.Guid)
                                .Query(guid))))));

        var modifiedResult = result.Documents.FirstOrDefault(x => x.Guid == guid);

        if (modifiedResult == null)
            return NotFound();

        var newDoc = JsonConvert.DeserializeObject<Entity>(JsonConvert.SerializeObject(modifiedResult));

        var fileToDelete = newDoc.File.FirstOrDefault(x => x.Guid == fileGuid);

        if (fileToDelete == null)
            return NotFound();

        newDoc.File.Remove(fileToDelete);

        await _fileStorage.RemoveObjectAsync(fileToDelete.FileBucket, new Guid(fileToDelete.Guid));

        await _elasticClient.IndexAsync(newDoc, idx => idx
                .Index("entity")
                .Id(newDoc.Guid)
                .Refresh(Refresh.WaitFor));

        return Ok();
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Any)]
    [Route("entity_report")]
    public async Task<IActionResult> GetEntityReport(string guid)
    {
        var result = await _elasticClient.SearchAsync<Entity>(sa => sa
            .Size(1)
            .PostFilter(pf => pf
                .Bool(b => b.
                    Must(must => must
                    .MatchPhrase(m => m
                        .Field(f => f.Guid)
                        .Query(guid))))));

        var reportResult = result.Documents.FirstOrDefault(x => x.Guid == guid);

        if (reportResult == null)
            return NotFound();

        var outReport = await _documentFormatter.CreateDocx(reportResult);

        return new FileStreamResult(outReport, "application/octet-stream") { FileDownloadName = reportResult.NormalizedFIO + ".docx" };
    }
}
