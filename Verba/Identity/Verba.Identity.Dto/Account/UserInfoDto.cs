namespace Verba.Identity.Dto.Account;

public class UserInfoDto
{
    public string Id { get; set; }

    public string Login { get; set; }

    public string CreatedDateTime { get; set; }

    public string Role { get; set; }

    public bool Unlocked { get; set; }
}
