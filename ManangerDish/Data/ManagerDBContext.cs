using Microsoft.EntityFrameworkCore;
using ManagerDish.Models;
using ManagerDish.Models.Enum;

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
            #region Role

            modelBuilder.Entity<Role>().HasData(
                new Role{ Id = RoleEnum.Admin, RoleName = "Admin", RoleDescription = "Admin"},
                new Role{ Id = RoleEnum.Manager, RoleName = "Manager", RoleDescription = "Manager"},
                new Role{ Id = RoleEnum.Kitchen, RoleName = "Kitchen", RoleDescription = "Kitchen"},
                new Role{ Id = RoleEnum.Staff, RoleName = "Staff", RoleDescription = "Staff"},
                new Role{ Id = RoleEnum.Guest, RoleName = "Guest", RoleDescription = "Guest"}
            );

            #endregion

            #region Account
            modelBuilder.Entity<Account>().HasData(new Account
            { 
                Id = 1,
                Name = "Admin",
                Email = "tronghahu@gmail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("trong123"),
                Avatar = "",
                roleId = RoleEnum.Admin,
            });
            #endregion

        }
    }
}
