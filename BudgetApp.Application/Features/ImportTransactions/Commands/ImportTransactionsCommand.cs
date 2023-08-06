using BudgetApp.Core.Features.Transactions.Commands;
using BudgetApp.Core.Features.Transactions.Models;
using MediatR;

namespace BudgetApp.Core.Features.ImportTransactions.Commands;

public class ImportTransactionsCommand : IRequest
{
    public List<TransactionModel> Transactions { get; init; }
}

public class ImportTransactionsCommandHandler : IRequestHandler<ImportTransactionsCommand>
{
    private readonly IMediator _mediator;

    public ImportTransactionsCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Handle(ImportTransactionsCommand request, CancellationToken cancellationToken)
    {
        //TODO this should be done in transaction
        foreach (var t in request.Transactions)
        {
            await _mediator.Send(new CreateTransactionCommand()
            {
                TransactionModel = t,
                UserId = t.UserId
            }, cancellationToken);
        }
    }
}