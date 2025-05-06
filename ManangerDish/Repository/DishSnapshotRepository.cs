using ManagerDish.Data;
using ManagerDish.Models;
using ManagerDish.Repository.IRepository;

namespace ManagerDish.Repository
{
    public class DishSnapshotRepository : Repository<DishSnapshot>, IDishSnapshotRepository
    {
        private readonly ManagerDBContext _context;
        public DishSnapshotRepository(ManagerDBContext context) : base(context)
        {
            _context = context;
        }
        public void Update(DishSnapshot entity)
        {
            _context.DishSnapshots.Update(entity);
        }
    }
}
