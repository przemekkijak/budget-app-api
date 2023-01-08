
namespace BudgetApp.Domain.Interfaces;

public interface IBaseRepository
{
}

public interface IBaseRepository<T> : IBaseRepository
{
    public Task<T> GetByIdAsync(int id);
    public abstract Task<T> CreateAsync(T entity);

}