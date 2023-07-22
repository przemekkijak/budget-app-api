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
    private readonly IImportTransactionSchemeRepository _importTransactionSchemeRepository;
    private readonly IMapper _mapper;

    public GetImportTransactionSchemesHandler(IImportTransactionSchemeRepository importTransactionSchemeRepository,
        IMapper mapper)
    {
        _importTransactionSchemeRepository = importTransactionSchemeRepository;
        _mapper = mapper;
    }

    public async Task<List<ImportTransactionSchemeModel>> Handle(GetImportTransactionSchemes request, CancellationToken cancellationToken)
    {
        var entities = await _importTransactionSchemeRepository.GetForUser(request.UserId);
        return entities.Select(a => _mapper.Map<ImportTransactionSchemeModel>(a)).ToList();
    }
}