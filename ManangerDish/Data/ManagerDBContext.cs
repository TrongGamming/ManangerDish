using Microsoft.EntityFrameworkCore;
using ManagerDish.Models;

namespace ManagerDish.Data
{
    public class ManagerDBContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Guest> Guests { get; set; } = null!;
        public DbSet<Dish> Dishes { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public DbSet<Table> Tables { get; set; } = null!;
        public DbSet<DishSnapshot> DishSnapshots { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;


        public ManagerDBContext(DbContextOptions<ManagerDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
