using ManagerDish.Data;
using ManagerDish.Repository.IRepository;

namespace ManagerDish.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ManagerDBContext _context;
        public IAccountRepository Account { get; private set; }
        public ITableRepository Table { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IDishRepository Dish { get; private set; }
        public IDishSnapshotRepository DishSnapshot { get; private set; }
        public IPaymentRepository Payment { get; private set; }
        public IGuestRepository Guest { get; private set; }
        public IOrderRepository Order { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IRefreshTokenRepository RefreshToken { get; private set; }
        public IRoleRepository Role { get; private set; }
        public UnitOfWork(ManagerDBContext context)
        {
            _context = context;
            Account = new AccountRepository(_context);
            Table = new TableRepository(_context);
            Category = new CategoryRepository(_context);
            Dish = new DishRepository(_context);
            DishSnapshot = new DishSnapshotRepository(_context);
            Payment = new PaymentRepository(_context);
            Guest = new GuestRepository(_context);
            Order = new OrderRepository(_context);
            OrderDetail = new OrderDetailRepository(_context);
            RefreshToken = new RefreshTokenRepository(_context);
            Role = new RoleRepository(_context);
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
