using BudgetApp.Domain;
using BudgetApp.Domain.Models;

namespace BudgetApp.Core.Interfaces.Services;

public interface ITransactionService
{
    Task<ExecutionResult> AddTransaction(int userId, AddTransactionModel model);
}