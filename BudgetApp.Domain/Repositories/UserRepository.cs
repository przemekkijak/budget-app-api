using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Interfaces.Repositories;
using Dommel;

namespace BudgetApp.Domain.Repositories;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{

    public UserRepository(AppSettings appSettings) : base(appSettings)
    {
    }

    public async Task<UserEntity?> GetByEmail(string email)
    {
        email = email.Trim().ToLower();
        using var con = CreateConnection();
        return (await con.SelectAsync<UserEntity>(a => a.Email == email)).SingleOrDefault();
    }
}