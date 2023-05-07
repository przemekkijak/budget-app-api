using BudgetApp.Core.Common;
using BudgetApp.Core.Features.Budgets.Models;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Interfaces.Repositories;
using MediatR;

namespace BudgetApp.Core.Features.Budgets.Commands;

public class CreateBudget : IRequest<ExecutionResult>
{
    public int UserId { get; init; }
    public BudgetModel? Budget { get; set; }
}

public class CreateBudgetHandler : IRequestHandler<CreateBudget, ExecutionResult>
{
    private readonly IBudgetRepository budgetRepository;

    private const string DefaultBudgetName = "Default";

    public CreateBudgetHandler(IBudgetRepository budgetRepository)
    {
        this.budgetRepository = budgetRepository;
    }
    
    public async Task<ExecutionResult> Handle(CreateBudget request, CancellationToken cancellationToken)
    {
        request.Budget ??= new BudgetModel()
        {
            Name = DefaultBudgetName,
            IsDefault = true,
            UserId = request.UserId
        };
        
        var existing = await budgetRepository.GetByName(request.UserId, request.Budget.Name);
        if (existing != null)
        {
            return new ExecutionResult<BudgetModel>(new ErrorInfo(ErrorCode.BudgetError,
                MessageCode.BudgetAlreadyExists));
        }
        
        var entity = new BudgetEntity
        {
            Name = request.Budget.Name,
            UserId = request.UserId,
            IsDefault = request.Budget.IsDefault,
            CreateDate = TimeService.Now,
            UpdateDate = TimeService.Now
        };

        await budgetRepository.CreateAsync(entity);
        return new ExecutionResult();
    }
}