using BudgetApp.Domain;
using BudgetApp.Domain.Models;

namespace BudgetApp.Core.Interfaces.Services;

public interface ITransactionService
{
    Task<ExecutionResult> UpdateTransaction(int userId, TransactionModel model);
    Task<List<TransactionModel>> GetTransactionsForBudget(int budgetId, bool currentMonthOnly);
    Task<ExecutionResult> DeleteTransaction(int userId, int transactionId);
}