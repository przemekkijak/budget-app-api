using System.Data;
using BudgetApp.Domain;
using BudgetApp.Domain.Interfaces;
using Dommel;
using Npgsql;

namespace BudgetApp.Data;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly AppSettings _appSettings;

    public BaseRepository(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    protected IDbConnection CreateConnection() => new NpgsqlConnection(_appSettings.ConnectionString);

    public async Task<T> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<T> CreateAsync(T entity)
    {
        using var con = CreateConnection();
        await con.InsertAsync(entity);
        return entity;
    }
}