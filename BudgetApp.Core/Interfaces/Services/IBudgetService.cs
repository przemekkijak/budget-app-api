using BudgetApp.Domain;
using BudgetApp.Domain.Models;

namespace BudgetApp.Core.Interfaces.Services;

public interface IBudgetService
{
    Task<ExecutionResult<BudgetModel>> GetDefault(int userId, bool currentMonthTransactionsOnly = false);
    Task<ExecutionResult<BudgetModel>> CreateBudget(int userId, BudgetModel budget);
}