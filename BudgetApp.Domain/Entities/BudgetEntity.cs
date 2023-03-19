﻿namespace BudgetApp.Domain.Entities;

public class BudgetEntity : EntityBase
{
    public int UserId { get; set; }

    public string Name { get; set; }

    public bool IsDefault { get; set; }

    public decimal Amount { get; set; }

    public IList<TransactionEntity> Transactions { get; set; }
}