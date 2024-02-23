using Verba.Stock.Domain.ModelsForPostgres;

namespace Verba.Stock.Domain.Models.CardNumbers;

public class CardNumbers : BaseEntity
{
    public string? CardNumber { get; set; }

    public string? Date { get; set; }

    public string? Count { get; set; }

    public string? Country { get; set; }
}
