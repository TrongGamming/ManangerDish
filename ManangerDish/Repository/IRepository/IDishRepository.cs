using ManagerDish.Models;

namespace ManagerDish.Repository.IRepository
{
    public interface IDishRepository : IRepository<Dish>
    {
        void Update(Dish entity);
    }
}
