using ManagerDish.Models;

namespace ManagerDish.Repository.IRepository
{
    public interface IGuestRepository : IRepository<Guest>
    {
        void Update(Guest entity);
    }
}
