using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.Transactions.Queries;

public class GetImportedTransactionHashList : IRequest<HashSet<string>>
{
    public int BudgetId { get; init; }
}

public class GetImportedTransactionHashListHandler : IRequestHandler<GetImportedTransactionHashList, HashSet<string>>
{
    private readonly ITransactionRepository transactionRepository;

    public GetImportedTransactionHashListHandler(ITransactionRepository transactionRepository)
    {
        this.transactionRepository = transactionRepository;
    }
    
    public async Task<HashSet<string>> Handle(GetImportedTransactionHashList request, CancellationToken cancellationToken)
    {
        return await transactionRepository.GetHashListForBudget(request.BudgetId);
    }
}