using Verba.Stock.Domain.ModelsForPostgres;

namespace Verba.Stock.Domain.Models.Snilses;

public class SnilsesHash : BaseEntity
{
    public string? SnilsHash { get; set; }

    public string? Date { get; set; }

    public string? Count { get; set; }

    public string? Country { get; set; }
}
