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

    public GetTransactionsForBudgetHandler(ITransactionRepository transactionRepository, IMapper mapper)
    {
        this.transactionRepository = transactionRepository;
        this.mapper = mapper;
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

        var entities = await transactionRepository.GetForBudget(request.BudgetId, startDate, endDate);
        
        var transactionModels = entities.Select(a => mapper.Map<TransactionModel>(a)).ToList();
        return transactionModels;
    }
}