using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Repositories.Interfaces;
using Dommel;

namespace BudgetApp.Domain.Repositories;

public class ImportTransactionSchemeRepository : BaseRepository<ImportTransactionSchemeEntity>, IImportTransactionSchemeRepository
{
    public ImportTransactionSchemeRepository(AppSettings appSettings) : base(appSettings)
    {
    }

    public async Task<List<ImportTransactionSchemeEntity>> GetForUser(int userId)
    {
        using var con = CreateConnection();
        return (await con.SelectAsync<ImportTransactionSchemeEntity>(a => a.UserId == userId)).ToList();
    }
}