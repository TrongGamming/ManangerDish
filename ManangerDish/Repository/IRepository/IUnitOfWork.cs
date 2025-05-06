using ManagerDish.Data;

namespace ManagerDish.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IAccountRepository Account { get; }
        ITableRepository Table { get; }
        ICategoryRepository Category { get; }
        IDishRepository Dish { get; }
        IDishSnapshotRepository DishSnapshot { get; }
        IPaymentRepository Payment { get; }
        IGuestRepository Guest { get; }
        IOrderRepository Order { get; }
        IOrderDetailRepository OrderDetail { get; }
        IRefreshTokenRepository RefreshToken { get; }
        IRoleRepository Role { get; }
        public Task Save();
    }
}
