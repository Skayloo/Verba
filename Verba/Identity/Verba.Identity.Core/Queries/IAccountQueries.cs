using Verba.Identity.Dto.Account;

namespace Verba.Identity.Core.Queries;

public interface IAccountQueries
{
    Task<UserInfoDto> GetAccountInfoAsync(string userId);

    Task<List<UserInfoDto>> GetAllAccountsAsync();
}
