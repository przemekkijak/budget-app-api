namespace BudgetApp.Models;

public class LoginResultModel
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }

    public UserModel User { get; set; }
}