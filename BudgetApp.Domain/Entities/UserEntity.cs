namespace BudgetApp.Domain.Entities;

public class UserEntity
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }
}