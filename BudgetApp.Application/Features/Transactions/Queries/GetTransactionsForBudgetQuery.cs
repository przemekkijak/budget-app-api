using AutoMapper;
using BudgetApp.Core.Features.Transactions.Models;
using BudgetApp.Core.Services;
using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.Transactions.Queries;

public class GetTransactionsForBudgetQuery : IRequest<List<TransactionModel>>
{
    public int BudgetId { get; init; }

    public bool CurrentMonthOnly { get; init; }
}

public class GetTransactionsForBudgetHandler : IRequestHandler<GetTransactionsForBudgetQuery, List<TransactionModel>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly IBankAccountRepository _bankAccountRepository;

    public GetTransactionsForBudgetHandler(ITransactionRepository transactionRepository, IMapper mapper, IBankAccountRepository bankAccountRepository)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<List<TransactionModel>> Handle(GetTransactionsForBudgetQuery request, CancellationToken cancellationToken)
    {
        DateTime? startDate = null;
        DateTime? endDate = null;

        if (request.CurrentMonthOnly)
        {
            var now = TimeService.Now;
            startDate = new DateTime(now.Year, now.Month, 1);
            endDate = new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
        }

        var transactionEntities = await _transactionRepository.GetForBudget(request.BudgetId, startDate, endDate);

        var bankAccountIds = transactionEntities.Select(a => a.BankAccountId).ToHashSet();
        var bankAccounts = await _bankAccountRepository.GetByIds(bankAccountIds);
        var bankAccountsDict = bankAccounts.ToDictionary(a => a.Id, a => a);

        foreach (var e in transactionEntities)
        {
            e.BankAccount = bankAccountsDict[e.BankAccountId];
        }
        
        var transactionModels = transactionEntities.Select(a => _mapper.Map<TransactionModel>(a)).ToList();
        return transactionModels
            .OrderByDescending(a => a.PaymentDate)
            .ThenByDescending(a => a.Id)
            .ToList();
    }
}