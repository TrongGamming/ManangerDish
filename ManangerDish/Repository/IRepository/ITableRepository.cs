using ManagerDish.Models;

namespace ManagerDish.Repository.IRepository
{
    public interface ITableRepository : IRepository<Table>
    {
        void Update(Table entity);
    }
}
