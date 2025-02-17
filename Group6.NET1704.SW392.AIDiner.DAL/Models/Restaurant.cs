using System;
using System.Collections.Generic;

namespace Group6.NET1704.SW392.AIDiner.DAL.Models
{
    public partial class Restaurant
    {
        public Restaurant()
        {
            Dishes = new HashSet<Dish>();
            Orders = new HashSet<Order>();
            Tables = new HashSet<Table>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<Dish> Dishes { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Table> Tables { get; set; }
    }
}
