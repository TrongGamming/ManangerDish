using ManagerDish.Models;
using ManagerDish.Data;
using ManagerDish.Repository.IRepository;

namespace ManagerDish.Repository
{
    public class DishRepository : Repository<Dish>, IDishRepository
    {
        private readonly ManagerDBContext _context;
        public DishRepository(ManagerDBContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Dish entity)
        {
            _context.Dishes.Update(entity);
        }
    }
}
