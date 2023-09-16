using BudgetApp.Core.Features.Budgets.Models;
using BudgetApp.Core.Features.Budgets.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

[Authorize]
[Controller]
public class BudgetsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public BudgetsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    public async Task<BudgetModel> GetDefaultBudget()
    {
        var budget = await _mediator.Send(new GetBudget()
        {
            UserId = UserId
        });

        return budget.Value ?? new BudgetModel();
    }
}