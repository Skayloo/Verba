using Verba.Stock.Domain.ModelsForPostgres;

namespace Verba.Stock.Domain.Models.AccountNumbers;

public class AccountNumbers : BaseEntity
{
    public string AccountNumber { get; set; }

    public string BIK { get; set; }

    public string Date { get; set; }

    public string Count { get; set; }

    public string Country { get; set; }
}
