using ManagerDish.Models;

namespace ManagerDish.Repository.IRepository
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail entity);
    }
}
