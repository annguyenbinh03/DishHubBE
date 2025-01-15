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
        public DbSet<ChatbotAI> ChatbotAIs { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=NGUYENTOAN;database=AIDiner;uid=sa;pwd=12345;TrustServerCertificate=True;MultipleActiveResultSets=True;");
        }
    }
}
