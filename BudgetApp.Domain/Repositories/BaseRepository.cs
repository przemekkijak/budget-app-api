using System.Data;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Repositories.Interfaces;
using Dommel;
using Npgsql;

namespace BudgetApp.Domain.Repositories;

public class BaseRepository : IBaseRepository
{
    
}

public class BaseRepository<T> : BaseRepository, IBaseRepository<T> where T : EntityBase
{
    private readonly AppSettings appSettings;

    protected BaseRepository(AppSettings appSettings)
    {
        this.appSettings = appSettings;
    }

    protected IDbConnection CreateConnection() => new NpgsqlConnection(appSettings.ConnectionString);

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        using var con = CreateConnection();
        return (await con.SelectAsync<T>(a => a.Id == id)).SingleOrDefault();
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        using var con = CreateConnection();
        var createdEntity = (int)await con.InsertAsync(entity);
        return await con.GetAsync<T>(createdEntity);
    }

    public virtual async Task<List<T>> GetByIds(IEnumerable<int> ids)
    {
        using var con = CreateConnection();
        return (await con.SelectAsync<T>(a => ids.Contains(a.Id))).ToList();
    }

    public virtual async Task<bool> UpdateAsync(T entity)
    {
        using var con = CreateConnection();
        return await con.UpdateAsync(entity);
    }

    public virtual async Task DeleteAsync(T entity)
    {
        using var con = CreateConnection();
        await con.DeleteAsync(entity);
    }
}