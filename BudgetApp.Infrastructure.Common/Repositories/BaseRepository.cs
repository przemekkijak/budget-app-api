using System.Data;
using BudgetApp.Domain;
using BudgetApp.Domain.Common.Entities;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Repositories.Interfaces;
using Dommel;
using Npgsql;

namespace BudgetApp.Infrastructure.Repositories;

public class BaseRepository : IBaseRepository
{
    
}

public class BaseRepository<T> : BaseRepository, IBaseRepository<T> where T : EntityBase
{
    private readonly AppSettings _appSettings;

    protected BaseRepository(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    protected IDbConnection CreateConnection() => new NpgsqlConnection(_appSettings.ConnectionString);

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