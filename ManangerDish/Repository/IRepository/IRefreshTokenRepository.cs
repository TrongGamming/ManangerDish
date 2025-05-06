using ManagerDish.Models;

namespace ManagerDish.Repository.IRepository
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        void Update(RefreshToken entity);
    }
}
