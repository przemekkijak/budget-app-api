using BudgetApp.Core.Features.Transactions.Models;
using BudgetApp.Core.Features.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

[Authorize]
[Controller]
[Route("api/transactions")]
public class TransactionsController : ApiControllerBase
{
    private readonly IMediator mediator;

    public TransactionsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("{budgetId:int}")]
    public async Task<List<TransactionModel>> GetForBudget(int budgetId)
    {
        return await mediator.Send(new GetTransactionsForBudget()
        {
            BudgetId = budgetId,
            CurrentMonthOnly = false
        });
    }
}