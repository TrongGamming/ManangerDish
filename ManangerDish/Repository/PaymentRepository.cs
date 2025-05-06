using ManagerDish.Data;
using ManagerDish.Models;
using ManagerDish.Repository.IRepository;

namespace ManagerDish.Repository
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly ManagerDBContext _db;
        public PaymentRepository(ManagerDBContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Payment entity)
        {
            _db.Payments.Update(entity);
        }
    }
}
