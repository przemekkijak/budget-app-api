using System.Data;
using BudgetApp.Domain;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Interfaces.Repositories;
using Dommel;
using Npgsql;

namespace BudgetApp.Data;

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

    public async Task<T?> GetByIdAsync(int id)
    {
        using var con = CreateConnection();
        return (await con.SelectAsync<T>(a => a.Id == id)).SingleOrDefault();
    }

    public async Task<T> CreateAsync(T entity)
    {
        using var con = CreateConnection();
        var createdEntity = (int)await con.InsertAsync(entity);
        return await con.GetAsync<T>(createdEntity);
    }
}