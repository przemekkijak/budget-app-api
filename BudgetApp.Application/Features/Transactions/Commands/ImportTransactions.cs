using System.Dynamic;
using BudgetApp.Core.Features.Transactions.Models;
using MediatR;

namespace BudgetApp.Core.Features.Transactions.Commands;

public class ImportTransactions : IRequest
{
    public int UserId { get; set; }

    public int BudgetId { get; set; }

    public ImportedTransactionScheme ImportedTransactionScheme { get; set; }

    public List<ExpandoObject> Transactions { get; set; }
}

public class ImportTransactionsHandler : IRequestHandler<ImportTransactions>
{
    public async Task Handle(ImportTransactions request, CancellationToken cancellationToken)
    {
        var transactionsToImport = new List<TransactionModel>();

        foreach (var t in request.Transactions)
        {
            transactionsToImport.Add(new TransactionModel()
            {
                BudgetId = request.BudgetId,
                UserId = request.UserId,
                Description = t.ElementAt(request.ImportedTransactionScheme.DescriptionIndex).Value.ToString(),
                CreateDate = TimeService.Now,
                Amount = decimal.Parse(t.ElementAt(request.ImportedTransactionScheme.AmountIndex).Value.ToString())
            });
        }

        Console.WriteLine("test");
    }
}