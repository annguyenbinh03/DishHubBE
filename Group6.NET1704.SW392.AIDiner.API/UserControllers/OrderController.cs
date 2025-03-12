using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Group6.NET1704.SW392.AIDiner.Services.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Group6.NET1704.SW392.AIDiner.API.UserControllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpGet("{id}")]
        public async Task<ResponseDTO> GetByOrderId(int id)
        {
            return await _orderService.GetByOrderId(id);
        }
        ///
        //[HttpPost]
        //public async Task<ResponseDTO> CreateOrder(CreateOrderDTO request)
        //{
        //    return await _orderService.CreateOrder(request);
        //}
        [HttpPost]
        public async Task<ResponseDTO> CreateOrderByTable(CreateOrderByTableDTO request)
        {
            return await _orderService.CreateOrderByTable(request);
        }
    }
}
