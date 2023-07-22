using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.Transactions.Queries;

public class GetImportedTransactionHashList : IRequest<HashSet<string>>
{
    public int BudgetId { get; init; }
}

public class GetImportedTransactionHashListHandler : IRequestHandler<GetImportedTransactionHashList, HashSet<string>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetImportedTransactionHashListHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    
    public async Task<HashSet<string>> Handle(GetImportedTransactionHashList request, CancellationToken cancellationToken)
    {
        return await _transactionRepository.GetHashListForBudget(request.BudgetId);
    }
}