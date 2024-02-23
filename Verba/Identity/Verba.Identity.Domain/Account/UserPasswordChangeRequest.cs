using System.ComponentModel.DataAnnotations;

namespace Verba.Identity.Domain.Account;

public class UserPasswordChangeRequest
{
    [Required]
    public string NewPassword { get; set; }
}
