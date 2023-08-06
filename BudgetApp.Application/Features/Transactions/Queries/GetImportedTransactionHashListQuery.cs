using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.Transactions.Queries;

public class GetImportedTransactionHashListQuery : IRequest<HashSet<string>>
{
    public int BudgetId { get; init; }
}

public class GetImportedTransactionHashListHandler : IRequestHandler<GetImportedTransactionHashListQuery, HashSet<string>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetImportedTransactionHashListHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    
    public async Task<HashSet<string>> Handle(GetImportedTransactionHashListQuery request, CancellationToken cancellationToken)
    {
        return await _transactionRepository.GetHashListForBudget(request.BudgetId);
    }
}