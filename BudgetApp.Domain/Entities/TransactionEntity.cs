using BudgetApp.Domain.Enums;
using BudgetApp.Domain.Objects;

namespace BudgetApp.Domain.Entities;

public class TransactionEntity : EntityBase
{
    public int BudgetId { get; set; } //TODO Add FK to Budget table

    public decimal Amount { get; set; }

    public TransactionStatusEnum StatusEnum { get; set; }

    public int UserId { get; set; } //TODO add to database
}