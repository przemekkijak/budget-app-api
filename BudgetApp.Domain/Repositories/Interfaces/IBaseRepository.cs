
namespace BudgetApp.Domain.Interfaces.Repositories;

public interface IBaseRepository
{
}

public interface IBaseRepository<T> : IBaseRepository
{
    public Task<T?> GetByIdAsync(int id);
    public Task<T> CreateAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task DeleteAsync(T entity);

}