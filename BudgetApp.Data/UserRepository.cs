using BudgetApp.Domain;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Interfaces;

namespace BudgetApp.Data;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{

    public UserRepository(AppSettings appSettings) : base(appSettings)
    {
    }
}