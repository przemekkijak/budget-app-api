﻿using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Models;

public class TransactionModel
{
    public int BudgetId { get; init; }

    public decimal Amount { get; set; }

    public TransactionStatusEnum Status { get; set; }

    public int UserId { get; init; }

    public string Description { get; set; }
}