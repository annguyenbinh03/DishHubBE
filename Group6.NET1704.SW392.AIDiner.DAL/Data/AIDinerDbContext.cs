using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL.Data
{
    public class AIDinerDbContext : DbContext
    {
        public AIDinerDbContext()
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<Request> Requests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=NGUYENTOAN;database=AIDiner;uid=sa;pwd=12345;TrustServerCertificate=True;MultipleActiveResultSets=True;");
        }
    }
}
