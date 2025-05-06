using ManagerDish.Models;
using ManagerDish.Data;
using ManagerDish.Repository.IRepository;

namespace ManagerDish.Repository
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly ManagerDBContext _db;
        public RefreshTokenRepository(ManagerDBContext db) : base(db)
        {
            _db = db;
        }
        public void Update(RefreshToken entity)
        {
            _db.RefreshTokens.Update(entity);
        }
    }
}
