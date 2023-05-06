
namespace BudgetApp.Core.Features.Users.Models;

public class UserModel
{
    public int Id { get; set; }

    public string Email { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }
}