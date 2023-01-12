using BudgetApp.Core.Interfaces.Services;
using BudgetApp.Domain;
using BudgetApp.Domain.Common;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Interfaces.Repositories;
using BudgetApp.Domain.Models;

namespace BudgetApp.Core;

public class BudgetService : IBudgetService
{
    private readonly IBudgetRepository budgetRepository;

    public BudgetService(IBudgetRepository budgetRepository)
    {
        this.budgetRepository = budgetRepository;
    }

    public async Task<ExecutionResult<BudgetModel>> GetDefault(int userId)
    {
        var budget = await budgetRepository.GetDefault(userId);
        if (budget is null)
        {
            //TODO handle error
            return new ExecutionResult<BudgetModel>();
        }
        
        return new ExecutionResult<BudgetModel>(ModelFactory.Create(budget));
    }

    public async Task<ExecutionResult<BudgetModel>> CreateBudget(int userId, BudgetModel budget)
    {
        var existing = await budgetRepository.GetByName(userId, budget.Name);
        if (existing != null)
        {
            return new ExecutionResult<BudgetModel>(new ErrorInfo(ErrorCode.BudgetError,
                MessageCode.BudgetAlreadyExists));
        }
        
        var entity = new BudgetEntity()
        {
            Name = budget.Name,
            UserId = userId,
            IsDefault = budget.IsDefault,
            CreateDate = TimeService.Now,
            UpdateDate = TimeService.Now
        };

        var createdBudget = await budgetRepository.CreateAsync(entity);
        return new ExecutionResult<BudgetModel>(ModelFactory.Create(createdBudget));
    }
}