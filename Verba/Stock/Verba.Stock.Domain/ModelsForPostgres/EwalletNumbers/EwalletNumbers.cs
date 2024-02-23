using Verba.Stock.Domain.ModelsForPostgres;

namespace Verba.Stock.Domain.Models.EwalletNumber;

public class EwalletNumbers : BaseEntity
{
    public string? WalletNumber { get; set; }

    public string? PaymentSystemName { get; set; }

    public string? Date { get; set; }

    public string? Count { get; set; }

    public string? Country { get; set; }
}
