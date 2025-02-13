using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Common.DTO
{
    public class CreateOrderDTO
    {
        public int CustomerId { get; set; }
        public int TableId { get; set; }
        public decimal TotalAmount { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; } = null!;
    }
}
