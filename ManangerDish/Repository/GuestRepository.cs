using ManagerDish.Data;
using ManagerDish.Models;
using ManagerDish.Repository.IRepository;

namespace ManagerDish.Repository
{
    public class GuestRepository : Repository<Guest>, IGuestRepository
    {
        private readonly ManagerDBContext _context;
        public GuestRepository(ManagerDBContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Guest entity)
        {
            _context.Guests.Update(entity);
        }
    }
}
