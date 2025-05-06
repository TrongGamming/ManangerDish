using ManagerDish.Models;

namespace ManagerDish.Repository.IRepository
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        void Update(Payment entity);
    }
}
