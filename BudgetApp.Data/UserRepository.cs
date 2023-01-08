using BudgetApp.Domain;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Interfaces;

namespace BudgetApp.Data;

public class UserData : BaseData<UserEntity>, IUserRepository
{

    public UserData(AppSettings appSettings) : base(appSettings)
    {
    }
}