using BudgetApp.Domain;
using BudgetApp.Domain.Entities;
using Dapper;
using Npgsql;

namespace BudgetApp.Data;

public class UserData
{
    private readonly AppSettings _appSettings;

    public UserData(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    public async Task<UserEntity> GetUserById(int id)
    {
        using (var con = new NpgsqlConnection(_appSettings.ConnectionString))
        {
            var test = con.QueryFirst<UserEntity>("SELECT * FROM users");
            Console.WriteLine(test);
            return test;
        }
    }
}