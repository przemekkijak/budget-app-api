using BudgetApp.Domain.Entities;

namespace BudgetApp.Domain;

public class LoginResult
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }

    public UserEntity User { get; set; }
}