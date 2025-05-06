using ManagerDish.Models;
using Newtonsoft.Json.Bson;

namespace ManagerDish.Repository.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order entity);
    }
}
