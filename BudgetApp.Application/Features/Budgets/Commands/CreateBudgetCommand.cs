using BudgetApp.Core.Common;
using BudgetApp.Core.Features.Budgets.Models;
using BudgetApp.Core.Services;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.Budgets.Commands;

public class CreateBudgetCommand : IRequest<ExecutionResult>
{
    public int UserId { get; init; }
    public BudgetModel? Budget { get; set; }
}

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, ExecutionResult>
{
    private readonly IBudgetRepository _budgetRepository;

    private const string DefaultBudgetName = "Default";

    public CreateBudgetCommandHandler(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }
    
    public async Task<ExecutionResult> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        request.Budget ??= new BudgetModel()
        {
            Name = DefaultBudgetName,
            IsDefault = true,
            UserId = request.UserId
        };
        
        var existing = await _budgetRepository.GetByName(request.UserId, request.Budget.Name);
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

        await _budgetRepository.CreateAsync(entity);
        return new ExecutionResult();
    }
}