using ManagerDish.Models;

namespace ManagerDish.Repository.IRepository
{
    public interface IRoleRepository : IRepository<Role>
    {
        void Update(Role entity);
    }
}
