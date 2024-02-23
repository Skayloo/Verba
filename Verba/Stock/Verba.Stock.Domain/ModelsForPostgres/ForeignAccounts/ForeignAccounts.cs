using Verba.Stock.Domain.ModelsForPostgres;

namespace Verba.Stock.Domain.Models.ForeignAccounts;

public class ForeignAccounts : BaseEntity
{
    public string? AccountNumber { get; set; }

    public string? Description { get; set; }
}
