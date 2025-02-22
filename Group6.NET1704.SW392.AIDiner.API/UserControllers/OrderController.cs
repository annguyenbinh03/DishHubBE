using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Group6.NET1704.SW392.AIDiner.API.UserControllers
{
    [Route("api/orders")]
    [ApiController]
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

        [HttpPost]
        public async Task<ResponseDTO> CreateOrder(CreateOrderDTO request)
        {
            return await _orderService.CreateOrder(request);
        }
        [HttpPost("table")]
        public async Task<ResponseDTO> CreateOrderByTable(CreateOrderByTableDTO request)
        {
            return await _orderService.CreateOrderByTable(request);
        }
    }
}
