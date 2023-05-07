using MediatR;

namespace BudgetApp.Core.Features.Transactions.Commands;

public class ImportTransactions : IRequest
{
    public byte[] FileBytes { get; set; }
}

public class ImportTransactionsHandler : IRequestHandler<ImportTransactions>
{
    public Task Handle(ImportTransactions request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}