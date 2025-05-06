using ManagerDish.Data;
using ManagerDish.Models;
using ManagerDish.Repository.IRepository;

namespace ManagerDish.Repository
{
    public class TableRepository : Repository<Table>, ITableRepository
    {
        private readonly ManagerDBContext _db;
        public TableRepository(ManagerDBContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Table entity)
        {
            _db.Tables.Update(entity);
        }
    }
}
