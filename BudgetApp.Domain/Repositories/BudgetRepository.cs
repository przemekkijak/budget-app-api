using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Interfaces.Repositories;
using Dommel;

namespace BudgetApp.Domain.Repositories;

public class BudgetRepository : BaseRepository<BudgetEntity>, IBudgetRepository
{
    public BudgetRepository(AppSettings appSettings) : base(appSettings)
    {
    }

    public async Task<BudgetEntity?> GetDefault(int userId)
    {
        using var con = CreateConnection();
        return (await con.SelectAsync<BudgetEntity>(a => a.UserId == userId && a.IsDefault)).SingleOrDefault();
    }

    public async Task<BudgetEntity?> GetByName(int userId, string name)
    {
        using var con = CreateConnection();
        return (await con.SelectAsync<BudgetEntity>(a => a.UserId == userId && a.Name == name)).FirstOrDefault();
    }
}