using Verba.Stock.Domain.ModelsForPostgres;

namespace Verba.Stock.Domain.Models.Lots;

public class Lots : BaseEntity
{
    public string? DateOfRequest { get; set; }

    public string? NameOfOrganization { get; set; }

    public string? FioOfInn { get; set; }

    public string? AddressOrAccountNumber { get; set; }

    public string? FioOfForeigner { get; set; }
}
