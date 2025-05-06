using ManagerDish.Models;

namespace ManagerDish.Repository.IRepository
{
    public interface IDishSnapshotRepository : IRepository<DishSnapshot>
    {
        void Update(DishSnapshot entity);
    }
}
