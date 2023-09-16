using BudgetApp.Domain.Common.Entities;

namespace BudgetApp.Domain.Entities;

public class BankAccountEntity : EntityBase
{
    public virtual string Name { get; set; }

    public virtual string Currency { get; set; }

    public virtual int UserId { get; init; }

    public virtual int BudgetId { get; init; }

    public virtual int NumberSuffix { get; set; }

    public virtual decimal Amount { get; set; }

    public virtual bool IsDefault { get; set; }

    public virtual bool IsDeleted { get; set; }
}