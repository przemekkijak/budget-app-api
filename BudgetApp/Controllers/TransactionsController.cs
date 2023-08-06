using BudgetApp.Core.Features.Transactions.Commands;
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
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{budgetId:int}")]
    public async Task<List<TransactionModel>> GetForBudget(int budgetId)
    {
        return await _mediator.Send(new GetTransactionsForBudgetQuery()
        {
            BudgetId = budgetId,
            CurrentMonthOnly = false
        });
    }

    [HttpPost]
    public async Task CreateTransaction([FromBody] TransactionModel model)
    {
        await _mediator.Send(new CreateTransactionCommand()
        {
            TransactionModel = model,
            UserId = UserId
        });
    }
}