using ManagerDish.Data;
using ManagerDish.Models;
using ManagerDish.Repository.IRepository;

namespace ManagerDish.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ManagerDBContext _context;
        public CategoryRepository(ManagerDBContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Category entity)
        {
            _context.Categories.Update(entity);
        }
    }
}
