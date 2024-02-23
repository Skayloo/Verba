using Verba.Stock.Domain.ModelsForPostgres;

namespace Verba.Stock.Domain.Models.Terrorists;

public class Terrorists : BaseEntity
{
    public string? FIO { get; set; }

    public string? DateOfBirth { get; set; }

    public string? Inn { get; set; }

    public string? Ogrn { get; set; }

    public string? DUL { get; set; }

    public string? RegistrationAddress { get; set; }

    public string? DateOfRequest { get; set; }

    public string? DateOfControlStart { get; set; }

    public string? DateOfControlEnd { get; set; }

    public string? Note { get; set; }
}
