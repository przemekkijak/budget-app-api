using BudgetApp.Domain.Entities;

namespace BudgetApp.Domain.Repositories.Interfaces;

public interface IImportTransactionSchemeRepository : IBaseRepository<ImportTransactionSchemeEntity>
{
    Task<List<ImportTransactionSchemeEntity>> GetForUser(int userId);
}