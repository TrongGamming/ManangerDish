using ManagerDish.Data;
using ManagerDish.Models;
using ManagerDish.Repository.IRepository;

namespace ManagerDish.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ManagerDBContext _db;
        public OrderDetailRepository(ManagerDBContext db) : base(db)
        {
            _db = db;
        }
        public void Update(OrderDetail entity)
        {
            _db.OrderDetails.Update(entity);
        }
    }
}
