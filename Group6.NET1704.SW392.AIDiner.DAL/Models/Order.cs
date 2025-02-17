using System;
using System.Collections.Generic;

namespace Group6.NET1704.SW392.AIDiner.DAL.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Payments = new HashSet<Payment>();
            Requests = new HashSet<Request>();
        }

        public int Id { get; set; }
        public int TableId { get; set; }
        public decimal TotalAmount { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; } = null!;
        public int RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; } = null!;
        public virtual Table Table { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
