using System.Linq.Expressions;
using ManagerDish.Data;
using Microsoft.EntityFrameworkCore;
using ManagerDish.Repository.IRepository;

namespace ManagerDish.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        public Repository(ManagerDBContext context)
        {
            _dbSet = context.Set<T>();
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.FirstOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }

        public async Task Create(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task RemoveRange(IEnumerable<T> entitys)
        {
            _dbSet.RemoveRange(entitys);
        }
    }
}
