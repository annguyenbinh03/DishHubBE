using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("Payment")]
    public class Payment
    {
        [Key]
        public Guid PaymentID { get; set; }
        public string? PaymentCode { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public Guid OrderID { get; set; }
        [ForeignKey(nameof(OrderID))]
        public Order? Order { get; set; }
    }
}
