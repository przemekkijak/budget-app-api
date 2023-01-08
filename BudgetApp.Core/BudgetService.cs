using BudgetApp.Core.Interfaces.Services;
using BudgetApp.Domain;
using BudgetApp.Domain.Interfaces.Repositories;
using BudgetApp.Domain.Models;

namespace BudgetApp.Core;

public class BudgetService : IBudgetService
{
    private readonly IBudgetRepository _budgetRepository;

    public BudgetService(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task<ExecutionResult<BudgetModel>> GetDefault(int userId)
    {
        var budget = await _budgetRepository.GetDefault(userId);
        if (budget is null)
        {
            //TODO handle error
            return new ExecutionResult<BudgetModel>();
        }
        
        return new ExecutionResult<BudgetModel>(ModelFactory.Create(budget));
    }
}