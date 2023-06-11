using AutoMapper;
using BudgetApp.Core.Features.ImportTransactions.Models;
using BudgetApp.Core.Services;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.ImportTransactions.Commands;

public class CreateImportTransactionScheme : IRequest
{
    public ImportTransactionSchemeModel ImportTransactionSchemeModel { get; init; }
}

public class CreateImportTransactionSchemeHandler : IRequestHandler<CreateImportTransactionScheme>
{
    private readonly IImportTransactionSchemeRepository importTransactionSchemeRepository;
    private readonly IMapper mapper;

    public CreateImportTransactionSchemeHandler(IImportTransactionSchemeRepository importTransactionSchemeRepository,
        IMapper mapper)
    {
        this.importTransactionSchemeRepository = importTransactionSchemeRepository;
        this.mapper = mapper;
    }

    public async Task Handle(CreateImportTransactionScheme request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<ImportTransactionSchemeEntity>(request.ImportTransactionSchemeModel);
        //TODO-PK test if mapping 

        entity.CreateDate = TimeService.Now;
        entity.UpdateDate = TimeService.Now;

        await importTransactionSchemeRepository.CreateAsync(entity);
    }
}