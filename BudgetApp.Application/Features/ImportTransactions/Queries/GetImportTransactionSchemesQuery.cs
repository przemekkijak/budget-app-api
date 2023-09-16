using AutoMapper;
using BudgetApp.Core.Features.ImportTransactions.Models;
using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.ImportTransactions.Queries;

public class GetImportTransactionSchemesQuery : IRequest<List<ImportTransactionSchemeModel>>
{
    public int UserId { get; init; }
}

public class GetImportTransactionSchemesHandler : IRequestHandler<GetImportTransactionSchemesQuery,
        List<ImportTransactionSchemeModel>>
{
    private readonly IImportTransactionSchemeRepository _importTransactionSchemeRepository;
    private readonly IMapper _mapper;

    public GetImportTransactionSchemesHandler(IImportTransactionSchemeRepository importTransactionSchemeRepository,
        IMapper mapper)
    {
        _importTransactionSchemeRepository = importTransactionSchemeRepository;
        _mapper = mapper;
    }

    public async Task<List<ImportTransactionSchemeModel>> Handle(GetImportTransactionSchemesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _importTransactionSchemeRepository.GetForUser(request.UserId);
        return entities.Select(a => _mapper.Map<ImportTransactionSchemeModel>(a)).ToList();
    }
}