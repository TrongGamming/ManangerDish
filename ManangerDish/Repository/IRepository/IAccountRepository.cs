using ManagerDish.Models;

namespace ManagerDish.Repository.IRepository
{
    public interface IAccountRepository : IRepository<Account>
    {
        void Update(Account entity);
    }
       
}
