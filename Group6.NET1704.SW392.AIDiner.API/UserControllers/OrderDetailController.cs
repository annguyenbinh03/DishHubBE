using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Group6.NET1704.SW392.AIDiner.API.UserControllers
{
    [Route("api")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        public IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetOrderDetailByOrderID([FromQuery] int orderId)
        {
            var response = await _orderDetailService.GetOrderDetailByOrderID(orderId);
            return Ok(response);
        }

        [HttpPost("{id}/details")]
        public async Task<IActionResult> AddDishToOrder([FromRoute] int id, [FromBody] List<DishRequestDTO> dishes)
        {
            var response = await _orderDetailService.AddDishToOrder(id, dishes);
            return Ok(response);
        }
    }
}