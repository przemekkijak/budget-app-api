using AutoMapper;
using BudgetApp.Core.Features.Transactions.Models;
using BudgetApp.Domain.Interfaces.Repositories;
using MediatR;

namespace BudgetApp.Core.Features.Transactions.Queries;

public class GetTransactionsForBudget : IRequest<List<TransactionModel>>
{
    public int BudgetId { get; init; }

    public bool CurrentMonthOnly { get; init; }
}

public class GetTransactionsForBudgetHandler : IRequestHandler<GetTransactionsForBudget, List<TransactionModel>>
{
    private readonly ITransactionRepository transactionRepository;
    private readonly IMapper mapper;
    private readonly IBankAccountRepository bankAccountRepository;

    public GetTransactionsForBudgetHandler(ITransactionRepository transactionRepository, IMapper mapper, IBankAccountRepository bankAccountRepository)
    {
        this.transactionRepository = transactionRepository;
        this.mapper = mapper;
        this.bankAccountRepository = bankAccountRepository;
    }

    public async Task<List<TransactionModel>> Handle(GetTransactionsForBudget request, CancellationToken cancellationToken)
    {
        DateTime? startDate = null;
        DateTime? endDate = null;

        if (request.CurrentMonthOnly)
        {
            var now = TimeService.Now;
            startDate = new DateTime(now.Year, now.Month, 1);
            endDate = new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
        }

        var transactionEntities = await transactionRepository.GetForBudget(request.BudgetId, startDate, endDate);

        var bankAccountIds = transactionEntities.Select(a => a.BankAccountId).ToHashSet();
        var bankAccounts = await bankAccountRepository.GetByIds(bankAccountIds);
        var bankAccountsDict = bankAccounts.ToDictionary(a => a.Id, a => a);

        foreach (var e in transactionEntities)
        {
            e.BankAccount = bankAccountsDict[e.BankAccountId];
        }
        
        var transactionModels = transactionEntities.Select(a => mapper.Map<TransactionModel>(a)).ToList();
        return transactionModels;
    }
}