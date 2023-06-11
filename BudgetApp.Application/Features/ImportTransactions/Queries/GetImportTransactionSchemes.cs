using AutoMapper;
using BudgetApp.Core.Features.ImportTransactions.Models;
using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.ImportTransactions.Queries;

public class GetImportTransactionSchemes : IRequest<List<ImportTransactionSchemeModel>>
{
    public int UserId { get; init; }
}

public class GetImportTransactionSchemesHandler : IRequestHandler<GetImportTransactionSchemes,
        List<ImportTransactionSchemeModel>>
{
    private readonly IImportTransactionSchemeRepository importTransactionSchemeRepository;
    private readonly IMapper mapper;

    public GetImportTransactionSchemesHandler(IImportTransactionSchemeRepository importTransactionSchemeRepository,
        IMapper mapper)
    {
        this.importTransactionSchemeRepository = importTransactionSchemeRepository;
        this.mapper = mapper;
    }

    public async Task<List<ImportTransactionSchemeModel>> Handle(GetImportTransactionSchemes request, CancellationToken cancellationToken)
    {
        var entities = await importTransactionSchemeRepository.GetForUser(request.UserId);
        return entities.Select(a => mapper.Map<ImportTransactionSchemeModel>(a)).ToList();
    }
}