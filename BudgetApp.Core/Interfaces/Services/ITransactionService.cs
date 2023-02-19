using BudgetApp.Domain;
using BudgetApp.Domain.Models;

namespace BudgetApp.Core.Interfaces.Services;

public interface ITransactionService
{
    Task<ExecutionResult> AddTransaction(int userId, AddTransactionModel model);
    Task<ExecutionResult<bool>> UpdateTransaction(int userId, int transactionId, AddTransactionModel model);
    Task<List<TransactionModel>> GetTransactionsForBudget(int budgetId);
}