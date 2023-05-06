
namespace BudgetApp.Domain.Entities;

public class UserEntity : EntityBase
{
    public string Email { get; set; }
    
    public string PasswordHash { get; set; }

}