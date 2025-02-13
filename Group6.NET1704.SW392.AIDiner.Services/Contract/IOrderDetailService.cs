using Group6.NET1704.SW392.AIDiner.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.Contract
{
    public interface IOrderDetailService
    {
        public Task<ResponseDTO> GetOrderDetailByOrderID(int orderId);
        public Task<ResponseDTO> AddDishToOrder(int orderId, List<DishRequestDTO> dishes);

    }
}
