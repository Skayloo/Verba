using Verba.Stock.Domain.ModelsForPostgres;

namespace Verba.Stock.Domain.Models.Inn;

public class Inns : BaseEntity
{
    public string? Inn { get; set; }

    public string? Date { get; set; }

    public string? Count { get; set; }

    public string? Country { get; set; }
}
