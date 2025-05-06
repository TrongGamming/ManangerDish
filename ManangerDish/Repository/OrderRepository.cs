using ManagerDish.Data;
using ManagerDish.Models;
using ManagerDish.Repository.IRepository;

namespace ManagerDish.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ManagerDBContext _db;
        public OrderRepository(ManagerDBContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Order entity)
        {
            _db.Orders.Update(entity);
        }
    }
}
