using BudgetApp.Core.Features.BankAccounts.Models;
using BudgetApp.Core.Features.BankAccounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

[Authorize]
[Controller]
[Route("api/bankaccounts")]
public class BankAccountsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public BankAccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{budgetId}")]
    public async Task<List<BankAccountModel>> GetForBudget(int budgetId)
    {
        return await _mediator.Send(new GetBankAccountsForBudgetQuery()
        {
            BudgetId = budgetId
        });
    }
}