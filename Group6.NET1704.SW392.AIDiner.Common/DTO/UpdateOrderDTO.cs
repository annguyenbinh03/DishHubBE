using Group6.NET1704.SW392.AIDiner.Common.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Common.DTO
{
    public class UpdateOrderDTO
    {
        public int TableId { get; set; }
        public bool PaymentStatus { get; set; }
        public string Status { get; set; }
        public List<UpdateOrder_DishDTO> Dishes { get; set; }
    }

    public class UpdateOrder_DishDTO
    {
        public int DishId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    }

    public class UpdateOrderResponseDTO
    {
        public int Id { get; set; }
        public string TableId { get; set; }
        public string TableName { get; set; }
        public decimal TotalAmount { get; set; }
        public bool PaymentStatus { get; set; }
        public string Status { get; set; }
        public List<OrderDetailResponseDTO> Dishes { get; set; }
    }


    public class OrderDetailResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }


}
