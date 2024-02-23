using Verba.Stock.Domain.ModelsForPostgres;

namespace Verba.Stock.Domain.Models.PhoneNumbers;

public class PhoneNumbers : BaseEntity
{
    public string? PhoneNumber { get; set; }

    public string? Date { get; set; }

    public string? Count { get; set; }

    public string? Country { get; set; }
}
