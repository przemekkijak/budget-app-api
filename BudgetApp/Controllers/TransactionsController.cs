using BudgetApp.Core.Interfaces.Services;
using BudgetApp.Domain;
using BudgetApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

[Authorize]
[Route("api/transactions")]

public class TransactionsController : ApiControllerBase
{
    private readonly ITransactionService transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        this.transactionService = transactionService;
    }

    [HttpPost]
    public async Task<ExecutionResult> AddTransaction([FromBody] AddTransactionModel model)
    {
        return await transactionService.AddTransaction(UserId, model);
    }

    [HttpPut]
    [Route("{transactionId}")]
    public async Task<ExecutionResult<bool>> UpdateTransactions(int transactionId, [FromBody] AddTransactionModel model)
    {
        var updated = await transactionService.UpdateTransaction(UserId, transactionId, model);
        return new ExecutionResult<bool>(updated);
    }

    [HttpDelete]
    [Route("{transactionId}")]
    public async Task<ExecutionResult> DeleteTransaction(int transactionId)
    {
        return await transactionService.DeleteTransaction(UserId, transactionId);
    }
}