using Group6.NET1704.SW392.AIDiner.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.Contract
{
    public interface IOrderService
    {
        public Task<ResponseDTO> GetAllOrder();
        public Task<ResponseDTO> GetByOrderId(int id);
        public Task<ResponseDTO> CreateOrder(CreateOrderDTO request);
        public Task<ResponseDTO> CreateOrderByTable(CreateOrderByTableDTO request);
        public Task<ResponseDTO> UpdateOrderByAdmin(int orderId, UpdateOrderDTO request);



    }
}
