using Verba.Stock.Domain.ModelsForPostgres;

namespace Verba.Stock.Domain.Models.Passports;

public class PassportsHash : BaseEntity
{
    public string? PassportHash { get; set; }

    public string? Date { get; set; }

    public string? Count { get; set; }

    public string? Country { get; set; }
}
