using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.Request;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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

        [HttpGet("orders/{orderId}/details")]
        public async Task<IActionResult> GetOrderDetailByOrderID([FromRoute] int orderId)
        {
            var response = await _orderDetailService.GetOrderDetailByOrderID(orderId);
            return Ok(response);
        }

        [HttpGet("orders/test")]
        public async Task<IActionResult> GetForTest([FromRoute] int restaurantId)
        {
            var response = await _orderDetailService.getRestaurantCurrentOrderDetail(restaurantId);
            return Ok(response);
        }

        [HttpPost("orders/{orderId}/details")]
        public async Task<IActionResult> AddDishToOrder([FromRoute] int orderId, [FromBody] List<DishRequestDTO> dishes)
        {
            var response = await _orderDetailService.AddDishToOrder(orderId, dishes);
            return Ok(response);
        }

        [HttpPatch("orders/details/{orderId}")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int orderId, [FromBody] ChangeOrderDetailStatusRequest request)
        {
            var response = await _orderDetailService.ChangeStatus(orderId, request.status);
            return Ok(response);
        }
    }
}