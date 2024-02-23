using Elasticsearch.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using System.Text;
using Verba.Abstractions.FileStorage;
using Verba.Core.Application.Authorization;
using Verba.Stock.Domain.ModelsForElastic.Entities;
using Verba.Stock.Domain.ModelsForElastic.ToUpdate;
using Verba.Stock.Dto.Guid;

namespace Verba.Stock.Core.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/persons")]
[ApiController]
public class SearchPersonsController : ControllerBase
{
    private readonly IElasticClient _elasticClient;

    public SearchPersonsController(IElasticClient elasticClient, ILogger<SearchPersonsController> logger, IFileStorage fileStorage)
    {
        _elasticClient = elasticClient;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Any)]
    [Route("get_entities")]
    [ProducesResponseType(typeof(Entity), 200)]
    public async Task<IActionResult> GetEntities([FromHeader(Name = "Authorization")] string jwt, [FromQuery] string keyWord, int? page, int? size)
    {
        if (page == null)
            page = 1;
        if (size == null)
            size = 10;
        var result = await _elasticClient.SearchAsync<Entity>(
            s => s.Query(
                q => q.QueryString(
                    d => d.Query('*' + keyWord + '*')
                )
            )
            .From((page - 1) * size)
            .Size(10)
            .Aggregations(a => a
                .ValueCount("total_count", v => v
                    .Field(f => f.Guid)
                    )
                )
        );

        var totalCount = result.Aggregations?.ValueCount("total_count")?.Value ?? 0;
        return Ok(new { TotalPages = Math.Ceiling((double)(totalCount / size)), result.Documents });
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Any)]
    [Route("get_entity")]
    [ProducesResponseType(typeof(Entity), 200)]
    public async Task<IActionResult> GetEntity(string guid)
    {
        var result = await _elasticClient.SearchAsync<Entity>(sa => sa
            .Size(1)
            .PostFilter(pf => pf
                .Bool(b => b.
                    Must(must => must
                    .MatchPhrase(m => m
                        .Field(f => f.Guid)
                        .Query(guid))))));

        var singleResult = result.Documents.FirstOrDefault(x => x.Guid == guid);

        if (singleResult == null)
            return BadRequest(404);

        return Ok(singleResult);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Uploader)]
    [Route("add_entity")]
    public async Task<IActionResult> PostEntity([FromBody] Entity entity)
    {
        entity.Guid = Guid.NewGuid().ToString();
        entity.NormalizedFIO = (string.IsNullOrEmpty(entity.FirstName) ? "" : entity.FirstName.ToUpper()) +
            " " +
            (string.IsNullOrEmpty(entity.LastName) ? "" : entity.LastName.ToUpper()) +
            " " +
            (string.IsNullOrEmpty(entity.MiddleName) ? "" : entity.MiddleName.ToUpper());

        await _elasticClient.IndexAsync(entity, x => x.Refresh(Refresh.WaitFor));

        var outGuid = new GuidOutcome()
        {
            Guid = entity.Guid,
        };

        return Ok(outGuid);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.User)]
    [Route("export_file")]
    public async Task<IActionResult> GetEntityReport(string guid)
    {
        var entity = await _elasticClient.SearchAsync<Entity>(sa => sa
            .Size(1)
            .PostFilter(pf => pf
                .Bool(b => b.
                    Must(must => must
                    .MatchPhrase(m => m
                        .Field(f => f.Guid)
                        .Query(guid))))));

        var exportingResult = entity.Documents.FirstOrDefault(x => x.Guid == guid);

        if (exportingResult == null)
            return BadRequest();

        var jsonBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(exportingResult));

        MemoryStream memoryStream = new MemoryStream(jsonBytes);
        memoryStream.Position = 0;
        var fileContentResult = new FileStreamResult(memoryStream, "application/octet-stream") { FileDownloadName = "exported_data.json" };
        return fileContentResult;
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Uploader)]
    [Route("alter_entity")]
    public async Task<IActionResult> AlterEntity(string guid, [FromBody] EntityUpdate entity)
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

        newDoc.FirstName = entity.FirstName;
        newDoc.LastName = entity.LastName;
        newDoc.MiddleName = entity.MiddleName;
        newDoc.BirthDate = entity.BirthDate;

        newDoc.NormalizedFIO = (string.IsNullOrEmpty(entity.FirstName) ? "" : entity.FirstName.ToUpper()) +
            " " +
            (string.IsNullOrEmpty(entity.LastName) ? "" : entity.LastName.ToUpper()) +
            " " +
            (string.IsNullOrEmpty(entity.MiddleName) ? "" : entity.MiddleName.ToUpper());


        newDoc.FamilyStatus = entity.FamilyStatus;
        newDoc.SpouseFirstName = entity.SpouseFirstName;
        newDoc.SpouseLastName = entity.SpouseLastName;
        newDoc.SpouseMiddleName = entity.SpouseMiddleName;
        newDoc.SpouseBirthDate = entity.SpouseBirthDate;
        newDoc.ParticipanInArmedConflicts = entity.ParticipanInArmedConflicts;
        newDoc.Notes = entity.Notes;
        newDoc.Citizenship = entity.Citizenship;
        newDoc.Gender = entity.Gender;
        newDoc.Contacts = entity.Contacts;
        newDoc.RelationsWithIntelligenceAgencies = entity.RelationsWithIntelligenceAgencies;
        newDoc.SocialMediaProfiles = entity.SocialMediaProfiles;

        await _elasticClient.IndexAsync(newDoc, idx => idx
                .Index("entity")
                .Id(newDoc.Guid)
                .Refresh(Refresh.WaitFor));

        return Ok(newDoc);
    }


    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Uploader)]
    [Route("alter_entity_own_property")]
    public async Task<IActionResult> AlterEntityOwnProperty(string guid, [FromBody] OwnPropertyUpdate ownProperty)
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

        newDoc.OwnProperty = ownProperty.OwnProperty;

        await _elasticClient.IndexAsync(newDoc, idx => idx
                .Index("entity")
                .Id(newDoc.Guid)
                .Refresh(Refresh.WaitFor));

        return Ok(newDoc);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Uploader)]
    [Route("alter_entity_identity_document")]
    public async Task<IActionResult> AlterEntityIdentityDocument(string guid, [FromBody] IdentityDocumentUpdate identityDocument)
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

        newDoc.IdentityDocument = identityDocument.IdentityDocument;

        await _elasticClient.IndexAsync(newDoc, idx => idx
                .Index("entity")
                .Id(newDoc.Guid)
                .Refresh(Refresh.WaitFor));

        return Ok(newDoc);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Uploader)]
    [Route("alter_entity_hobby")]
    public async Task<IActionResult> AlterEntityHobby(string guid, [FromBody] HobbyUpdate hobby)
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

        newDoc.Hobby = hobby.Hobby;

        await _elasticClient.IndexAsync(newDoc, idx => idx
                .Index("entity")
                .Id(newDoc.Guid)
                .Refresh(Refresh.WaitFor));

        return Ok(newDoc);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Uploader)]
    [Route("alter_entity_language")]
    public async Task<IActionResult> AlterEntityLanguage(string guid, [FromBody] LanguageUpdate language)
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

        newDoc.Language = language.Language;

        await _elasticClient.IndexAsync(newDoc, idx => idx
                .Index("entity")
                .Id(newDoc.Guid)
                .Refresh(Refresh.WaitFor));

        return Ok(newDoc);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Uploader)]
    [Route("alter_entity_education")]
    public async Task<IActionResult> AlterEntityEducation(string guid, [FromBody] EducationUpdate education)
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

        newDoc.Education = education.Education;

        await _elasticClient.IndexAsync(newDoc, idx => idx
                .Index("entity")
                .Id(newDoc.Guid)
                .Refresh(Refresh.WaitFor));

        return Ok(newDoc);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Uploader)]
    [Route("alter_entity_arrival")]
    public async Task<IActionResult> AlterEntityArrival(string guid, [FromBody] ArrivalUpdate arrival)
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

        newDoc.Arrival = arrival.Arrival;

        await _elasticClient.IndexAsync(newDoc, idx => idx
                .Index("entity")
                .Id(newDoc.Guid)
                .Refresh(Refresh.WaitFor));

        return Ok(newDoc);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Uploader)]
    [Route("alter_entity_workplace")]
    public async Task<IActionResult> AlterEntityWorkplace(string guid, [FromBody] WorkplaceUpdate workplace)
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

        newDoc.Workplace = workplace.Workplace;

        await _elasticClient.IndexAsync(newDoc, idx => idx
                .Index("entity")
                .Id(newDoc.Guid)
                .Refresh(Refresh.WaitFor));

        return Ok(newDoc);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Uploader)]
    [Route("alter_entity_project")]
    public async Task<IActionResult> AlterEntityProject(string guid, [FromBody] ProjectUpdate project)
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

        newDoc.Project = project.Project;

        await _elasticClient.IndexAsync(newDoc, idx => idx
                .Index("entity")
                .Id(newDoc.Guid)
                .Refresh(Refresh.WaitFor));

        return Ok(newDoc);
    }

    [HttpDelete]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Uploader)]
    [Route("delete_entity")]
    public async Task<IActionResult> DeleteEntity(string guid)
    {
        var result = await _elasticClient.SearchAsync<Entity>(sa => sa
            .Size(1)
            .PostFilter(pf => pf
                .Bool(b => b.
                    Must(must => must
                    .MatchPhrase(m => m
                        .Field(f => f.Guid)
                        .Query(guid))))));

        var deletingResult = result.Documents.FirstOrDefault(x => x.Guid == guid);

        if (deletingResult == null)
            return BadRequest();

        var response = await _elasticClient.DeleteAsync<Entity>(deletingResult.Guid, d => d
                .Index("entity")
                .Refresh(Refresh.WaitFor));

        if(!response.IsValid) 
            return BadRequest(404);

        return Ok();
    }
}
