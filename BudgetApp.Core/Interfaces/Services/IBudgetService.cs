using BudgetApp.Domain;
using BudgetApp.Domain.Models;

namespace BudgetApp.Core.Interfaces.Services;

public interface IBudgetService
{
    Task<ExecutionResult<int>> GetDefaultBudgetId(int userId);
    Task<ExecutionResult<BudgetModel>> GetBudget(int userId, bool currentMonthTransactionsOnly = false);
    Task<ExecutionResult<BudgetModel>> CreateBudget(int userId, BudgetModel budget);
}