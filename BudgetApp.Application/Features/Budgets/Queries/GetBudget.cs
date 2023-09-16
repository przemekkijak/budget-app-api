using AutoMapper;
using BudgetApp.Application.Common.Models;
using BudgetApp.Core.Features.Budgets.Models;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.Budgets.Queries;

public class GetBudget : IRequest<ExecutionResult<BudgetModel?>>
{
    public int UserId { get; init; }
    public int? BudgetId { get; init; }
}

public class GetBudgetHandler : IRequestHandler<GetBudget, ExecutionResult<BudgetModel?>>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IMapper _mapper;

    public GetBudgetHandler(IBudgetRepository budgetRepository, IMapper mapper)
    {
        _budgetRepository = budgetRepository;
        _mapper = mapper;
    }
    
    public async Task<ExecutionResult<BudgetModel?>> Handle(GetBudget request, CancellationToken cancellationToken)
    {
        BudgetEntity? budget;
        
        if (request.BudgetId.HasValue)
        {
            budget = await _budgetRepository.GetByIdAsync(request.BudgetId.Value);
            if (budget is null)
            {
                //TODO handle error
                return new ExecutionResult<BudgetModel?>();
            }
        }
        else
        {
            budget = await _budgetRepository.GetDefault(request.UserId);
        }

        if (budget is not null)
        {
            var budgetModel = _mapper.Map<BudgetModel>(budget);
            return new ExecutionResult<BudgetModel?>(budgetModel);
        }

        return new ExecutionResult<BudgetModel?>();
    }
}