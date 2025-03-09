using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.Response;
using Group6.NET1704.SW392.AIDiner.Services.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Group6.NET1704.SW392.AIDiner.API.UserControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestSignalR : ControllerBase
    {
        private readonly IHubContext<OrderDetailHub> _hubContext;

        public TestSignalR(IHubContext<OrderDetailHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("notify-staff/{restaurantId}")]
        public async Task<IActionResult> NotifyStaffNewOrder(int restaurantId, [FromBody] OrderDetailHubResponse orderDetail)
        {
            if (orderDetail == null || restaurantId <= 0)
            {
                return BadRequest("Invalid restaurantId or order details.");
            }

            // Gửi thông báo đến staff của nhà hàng
            await _hubContext.Clients.Group(restaurantId.ToString()).SendAsync("ReceiveNewOrder", orderDetail);

            return Ok(new { Message = "Order notification sent successfully." });
        }
    }
}
