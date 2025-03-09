using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Common.DTO
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MethodId { get; set; }
        public string MethodName { get; set; }
        public string? TransactionCode { get; set; }
        public string? CreatedAt { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
    }

    public class PaymentQueryDTO
    {
        public int? RestaurantId { get; set; }
    }
}
