using BudgetApp.Domain.Entities;
using Lisek.Domain;

namespace BudgetApp.Domain.Interfaces.Services;

public interface IUserService
{
    Task<LoginResult?> RegisterUser(User model);
    Task<ExecutionResult<LoginResult>> AuthenticateUser(User model);
    Task<ExecutionResult<UserEntity>> GetProfile(int userId);

}