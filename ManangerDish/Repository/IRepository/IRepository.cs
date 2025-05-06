using System.Linq.Expressions;

namespace ManagerDish.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(Expression<Func<T,bool>> filter);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter);
        Task Create(T entity);
        Task Remove(T entity);
        Task RemoveRange(IEnumerable<T> entitys);
    }
}
