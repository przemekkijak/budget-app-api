using AutoMapper;
using BudgetApp.Core.Common;
using BudgetApp.Core.Features.Budgets.Models;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Interfaces.Repositories;
using MediatR;

namespace BudgetApp.Core.Features.Budgets.Queries;

public class GetBudget : IRequest<ExecutionResult<BudgetModel>>
{
    public int UserId { get; init; }
    public int? BudgetId { get; init; }
}

public class GetBudgetHandler : IRequestHandler<GetBudget, ExecutionResult<BudgetModel>>
{
    private readonly IBudgetRepository budgetRepository;
    private readonly IMapper mapper;

    public GetBudgetHandler(IBudgetRepository budgetRepository, IMapper mapper)
    {
        this.budgetRepository = budgetRepository;
        this.mapper = mapper;
    }
    
    public async Task<ExecutionResult<BudgetModel>> Handle(GetBudget request, CancellationToken cancellationToken)
    {
        BudgetEntity? budget;
        
        if (request.BudgetId.HasValue)
        {
            budget = await budgetRepository.GetByIdAsync(request.BudgetId.Value);
            if (budget is null)
            {
                //TODO handle error
                return new ExecutionResult<BudgetModel>();
            }
        }
        else
        {
            budget = await budgetRepository.GetDefault(request.UserId);
        }

        if (budget is not null)
        {
            var budgetModel = mapper.Map<BudgetModel>(budget);
            return new ExecutionResult<BudgetModel>(budgetModel);
        }

        return new ExecutionResult<BudgetModel>();
    }
}