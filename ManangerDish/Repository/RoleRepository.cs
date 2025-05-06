using ManagerDish.Data;
using ManagerDish.Models;
using ManagerDish.Repository.IRepository;

namespace ManagerDish.Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly ManagerDBContext _db;
        public RoleRepository(ManagerDBContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Role entity)

        {
            _db.Roles.Update(entity);
        }
    }
}
