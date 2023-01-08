using BudgetApp.Domain;
using BudgetApp.Domain.Models;

namespace BudgetApp.Core.Interfaces.Services;

public interface IBudgetService
{
    Task<ExecutionResult<BudgetModel>> GetDefault(int userId);
}