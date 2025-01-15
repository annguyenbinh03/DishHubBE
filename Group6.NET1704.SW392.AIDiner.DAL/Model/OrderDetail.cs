using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [Key]
        public Guid OrderDetailID { get; set; }
        public Guid Quantity { get; set; }
        public decimal Price { get; set; }
        public Guid OrderID { get; set; }
        [ForeignKey(nameof(OrderID))]
        public Order? Order { get; set; }
        public Guid DishID { get; set; }
        [ForeignKey(nameof(DishID))]
        public Dish? Dish { get; set; }
    }
}
