using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Common.DTO
{
    public class OrderDetailDTO
    {
        public int OrderId { get; set; }  
        public int Id { get; set; }  
        public int DishId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = null!;
    }



}

