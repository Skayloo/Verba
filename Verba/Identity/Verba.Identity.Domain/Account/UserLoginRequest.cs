using System.ComponentModel.DataAnnotations;

namespace Verba.Identity.Domain.Account;

public class UserLoginRequest
{
    [Required]
    public string Login { get; set; }

    [Required]
    public string Password { get; set; }
}
