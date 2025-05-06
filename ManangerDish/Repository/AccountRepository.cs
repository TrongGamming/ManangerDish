using ManagerDish.Data;
using ManagerDish.Models;
using ManagerDish.Repository.IRepository;

namespace ManagerDish.Repository
{
    public class AccountRepository :  Repository<Account>, IAccountRepository
    {
        private readonly ManagerDBContext _context;
        public AccountRepository(ManagerDBContext context) : base(context)
        {
        }
        public void Update(Account entity)
        {
            _context.Accounts.Update(entity);
        }
    }
}
