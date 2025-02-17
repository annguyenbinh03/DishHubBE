using System;
using System.Collections.Generic;

namespace Group6.NET1704.SW392.AIDiner.DAL.Models
{
    public partial class Table
    {
        public Table()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Qrcode { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; } = null!;
        public int RestaurantId { get; set; }
        public string? Password { get; set; }

        public virtual Restaurant Restaurant { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
