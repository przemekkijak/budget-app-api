using AutoMapper;
using BudgetApp.Application.Common.Services;
using BudgetApp.Core.Features.ImportTransactions.Models;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.ImportTransactions.Commands;

public class CreateImportTransactionSchemeCommand : IRequest<ImportTransactionSchemeModel>
{
    public ImportTransactionSchemeModel ImportTransactionSchemeModel { get; init; }
}

public class CreateImportTransactionSchemeCommandHandler : IRequestHandler<CreateImportTransactionSchemeCommand, ImportTransactionSchemeModel>
{
    private readonly IImportTransactionSchemeRepository _importTransactionSchemeRepository;
    private readonly IMapper _mapper;

    public CreateImportTransactionSchemeCommandHandler(IImportTransactionSchemeRepository importTransactionSchemeRepository,
        IMapper mapper)
    {
        _importTransactionSchemeRepository = importTransactionSchemeRepository;
        _mapper = mapper;
    }

    public async Task<ImportTransactionSchemeModel>Handle(CreateImportTransactionSchemeCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ImportTransactionSchemeEntity>(request.ImportTransactionSchemeModel);
        //TODO-PK test if mapping 

        entity.CreateDate = TimeService.Now;
        entity.UpdateDate = TimeService.Now;

        await _importTransactionSchemeRepository.CreateAsync(entity);

        return _mapper.Map<ImportTransactionSchemeModel>(entity);
    }
}