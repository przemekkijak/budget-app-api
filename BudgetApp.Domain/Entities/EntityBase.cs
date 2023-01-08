namespace BudgetApp.Domain.Entities;

public class EntityBase
{
    public virtual int Id { get; set; }
    public virtual DateTime CreateDate { get; set; }
}