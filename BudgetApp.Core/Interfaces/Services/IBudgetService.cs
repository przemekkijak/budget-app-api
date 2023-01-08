using BudgetApp.Domain;
using BudgetApp.Domain.Models;

namespace BudgetApp.Core.Interfaces.Services;

public interface IBudgetService
{
    Task<ExecutionResult<BudgetModel>> GetDefault(int userId);
    Task<ExecutionResult<BudgetModel>> CreateBudget(int userId, BudgetModel budget);
}