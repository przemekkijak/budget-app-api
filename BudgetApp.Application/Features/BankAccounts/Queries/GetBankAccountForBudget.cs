using AutoMapper;
using BudgetApp.Core.Features.BankAccounts.Models;
using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.BankAccounts.Queries;

public class GetBankAccountsForBudget : IRequest<List<BankAccountModel>>
{
    public int BudgetId { get; init; }
}

public sealed class GetBankAccountsForBudgetHandler : IRequestHandler<GetBankAccountsForBudget, List<BankAccountModel>>
{
    private readonly IBankAccountRepository bankAccountRepository;
    private readonly IMapper mapper;

    public GetBankAccountsForBudgetHandler(IBankAccountRepository bankAccountRepository, IMapper mapper)
    {
        this.bankAccountRepository = bankAccountRepository;
        this.mapper = mapper;
    }
    
    public async Task<List<BankAccountModel>> Handle(GetBankAccountsForBudget request, CancellationToken cancellationToken)
    {
        var bankAccounts = await bankAccountRepository.GetForBudget(request.BudgetId);
        return bankAccounts.Select(a => mapper.Map<BankAccountModel>(a)).ToList();
    }
}