using BudgetApp.Core.Features.ImportTransactions.Commands;
using BudgetApp.Core.Features.ImportTransactions.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

[Authorize]
[Controller]
[Route("api/transactions/import")]
public class TransactionsImportController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsImportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("scheme")]
    public async Task<ImportTransactionSchemeModel> PostImportScheme(ImportTransactionSchemeModel model)
    {
        return await _mediator.Send(new CreateImportTransactionSchemeCommand()
        {
            ImportTransactionSchemeModel = model
        });
    }
}