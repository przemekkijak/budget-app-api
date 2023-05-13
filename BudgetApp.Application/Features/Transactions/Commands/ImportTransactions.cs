using System.Dynamic;
using BudgetApp.Core.Features.Transactions.Models;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.Domain.Interfaces.Repositories;
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
    private readonly ITransactionRepository transactionRepository;

    public ImportTransactionsHandler(ITransactionRepository transactionRepository)
    {
        this.transactionRepository = transactionRepository;
    }
    
    public async Task Handle(ImportTransactions request, CancellationToken cancellationToken)
    {
        var transactionsToImport = new List<TransactionEntity>();

        foreach (var t in request.Transactions)
        {
            var entity = new TransactionEntity()
            {
                BudgetId = request.ImportedTransactionScheme.BankAccountId,
                UserId = request.UserId,
                BankAccountId = request.ImportedTransactionScheme.BankAccountId,
                Description = t.ElementAt(request.ImportedTransactionScheme.DescriptionIndex).Value.ToString(),
                CreateDate = TimeService.Now,
                Amount = decimal.Parse(t.ElementAt(request.ImportedTransactionScheme.AmountIndex).Value.ToString()),
                Status = TransactionStatusEnum.Completed
            };

            await transactionRepository.CreateAsync(entity);
        }

        
    }
}