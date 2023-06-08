using BudgetApp.Core.Features.Transactions.Models;
using MediatR;

namespace BudgetApp.Core.Features.Transactions.Commands;

public class ImportTransactions : IRequest
{
    public List<TransactionModel> Transactions { get; init; }
}

public class ImportTransactionsHandler : IRequestHandler<ImportTransactions>
{
    private readonly IMediator mediator;

    public ImportTransactionsHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    public async Task Handle(ImportTransactions request, CancellationToken cancellationToken)
    {
        //TODO this should be done in transaction
        foreach (var t in request.Transactions)
        {
            await mediator.Send(new CreateTransaction()
            {
                TransactionModel = t,
                UserId = t.UserId
            }, cancellationToken);
        }
    }
}