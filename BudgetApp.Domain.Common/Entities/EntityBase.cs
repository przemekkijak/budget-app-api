namespace BudgetApp.Domain.Common.Entities;

public class EntityBase
{
    public virtual int Id { get; set; }
    public virtual DateTime CreateDate { get; set; }
    public virtual DateTime UpdateDate { get; set; }
}