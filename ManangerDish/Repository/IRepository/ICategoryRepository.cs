using ManagerDish.Models;

namespace ManagerDish.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category entity);
    }
}
