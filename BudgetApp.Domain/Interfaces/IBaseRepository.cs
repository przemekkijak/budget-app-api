
namespace BudgetApp.Domain.Interfaces;

public interface IBaseRepository<T>
{
    public Task<T> GetByIdAsync(int id);
    public abstract Task<T> CreateAsync(T entity);

}