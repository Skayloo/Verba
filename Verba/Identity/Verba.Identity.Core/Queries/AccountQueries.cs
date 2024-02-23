using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Verba.Abstractions.Application.Settings;
using Verba.Identity.Dto.Account;

namespace Verba.Identity.Core.Queries;

public class AccountQueries : IAccountQueries
{
    private readonly IOptions<DbConnectionSettings> _options;

    public AccountQueries(IOptions<DbConnectionSettings> options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<List<UserInfoDto>> GetAllAccountsAsync()
    {
        using (var connection = new NpgsqlConnection(_options.Value.DbConnection))
        {
            await connection.OpenAsync();

            const string query = @"select u.id as Id, u.email as Login, u.created_date_time as CreatedDateTime, u.email_confirmed as Unlocked, r.name as Role
                                from users as u
                                left join user_roles as ur on ur.user_id = u.id
                                left join roles as r on r.id = ur.role_id";

            List<UserInfoDto> account = new List<UserInfoDto>();

            using (var multi = await connection.QueryMultipleAsync(query))
            {
                account = multi.Read<UserInfoDto>().ToList();
            }

            return account;
        }
    }

    public async Task<UserInfoDto> GetAccountInfoAsync(string userId)
    {
        using (var connection = new NpgsqlConnection(_options.Value.DbConnection))
        {
            await connection.OpenAsync();

            const string query = @"select u.id as Id, u.email as Login, u.created_date_time as CreatedDateTime, u.email_confirmed as Unlocked, r.name as Role
                                from users as u
                                left join user_roles as ur on ur.user_id = u.id
                                left join roles as r on r.id = ur.role_id
                                where u.id = @userId";

            UserInfoDto account = null;
            account = await connection.QueryFirstOrDefaultAsync<UserInfoDto>(query, new { userId });
            account.Role = account.Role.ToLower();
            return account;
        }
    }
}
