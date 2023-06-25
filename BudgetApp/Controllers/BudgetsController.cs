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
    private readonly IMediator mediator;

    public BudgetsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("")]
    public async Task<BudgetModel> GetDefaultBudget()
    {
        var budget = await mediator.Send(new GetBudget()
        {
            UserId = UserId
        });

        return budget.Value ?? new BudgetModel();
    }
}