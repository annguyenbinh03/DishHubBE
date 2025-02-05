using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public Guid OrderID { get; set; }
        public Guid TableID { get; set; }
        [ForeignKey(nameof(TableID))]
        public Table? Table { get; set; }
        public decimal TotalAmount { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime CreateAt { get; set; }
        public bool Status { get; set; }
    }
}
