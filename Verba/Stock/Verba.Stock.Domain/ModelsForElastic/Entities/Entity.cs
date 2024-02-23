using Nest;

namespace Verba.Stock.Domain.ModelsForElastic.Entities;

[ElasticsearchType(IdProperty = nameof(Guid))]
public class Entity
{
    [Keyword]
    public string Guid { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? MiddleName { get; set; }

    public string? BirthDate { get; set; }

    public string? NormalizedFIO { get; set; }

    public string? FamilyStatus { get; set; }

    public string? SpouseFirstName { get; set; }

    public string? SpouseLastName { get; set; }

    public string? SpouseMiddleName { get; set; }

    public string? SpouseBirthDate { get; set; }

    public string? ParticipanInArmedConflicts { get; set; }

    [Text(Index = false)]
    public string? Notes { get; set; }

    public string? Citizenship { get; set; }

    public string? Gender { get; set; }

    public string? Contacts { get; set; }

    public string? RelationsWithIntelligenceAgencies { get; set; }

    public string? SocialMediaProfiles { get; set; }

    public Avatar.Avatar Avatar { get; set; }

    public List<Properties.OwnProperty> OwnProperty { get; set; } = new List<Properties.OwnProperty>();

    public List<IdentityDocuments.IdentityDocument> IdentityDocument { get; set; } = new List<IdentityDocuments.IdentityDocument>();

    public List<Files.File> File { get; set; } = new List<Files.File>();

    public List<Hobbies.Hobby> Hobby { get; set; } = new List<Hobbies.Hobby>();

    public List<Languages.Language> Language { get; set; } = new List<Languages.Language>();

    public List<Educations.Education> Education { get; set; } = new List<Educations.Education>();

    public List<Arrivals.Arrival> Arrival { get; set; } = new List<Arrivals.Arrival>();

    public List<Workplaces.Workplace> Workplace { get; set; } = new List<Workplaces.Workplace>();

    public List<Projects.Project> Project { get; set; } = new List<Projects.Project>();
}
