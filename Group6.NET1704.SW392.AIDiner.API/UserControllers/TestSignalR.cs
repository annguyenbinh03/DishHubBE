using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Group6.NET1704.SW392.AIDiner.Services.Hubs;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Azure.Core;

namespace Group6.NET1704.SW392.AIDiner.API.UserControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestSignalR : ControllerBase
    {
        private readonly IHubContext<OrderDetailHub> _orderDetailHubContext;
        private readonly IHubContext<RequestHub> _requestHubContext;

        public TestSignalR(IHubContext<OrderDetailHub> hubContext, IHubContext<RequestHub> requestHubContext)
        {
            _orderDetailHubContext = hubContext;
            _requestHubContext = requestHubContext;
        }

        //[HttpPost("notify-staff/create-new-orderdetail/{restaurantId}")]
        //public async Task<IActionResult> NotifyStaffNewOrder(int restaurantId, [FromBody] OrderDetailHubResponse orderDetail)
        //{
        //    if (orderDetail == null || restaurantId <= 0)
        //    {
        //        return BadRequest("Invalid restaurantId or order details.");
        //    }

        //    // Gửi thông báo đến staff của nhà hàng
        //    await _orderDetailHubContext.Clients.Group(restaurantId.ToString()).SendAsync("ReceiveNewOrder", orderDetail);

        //    return Ok(new { Message = "Order notification sent successfully." });
        //}

        //[HttpPost("notify-staff/create-new-request/{restaurantId}")]
        //public async Task<IActionResult> NotifyStaffNewOrder(int restaurantId, [FromBody] RequestHubResponse request)
        //{
        //    if (request == null || restaurantId <= 0)
        //    {
        //        return BadRequest("Invalid restaurantId or order details.");
        //    }

        //    // Gửi thông báo đến staff của nhà hàng
        //    await _requestHubContext.Clients.Group(restaurantId.ToString()).SendAsync("ReceiveNewRequest", request);

        //    return Ok(new { Message = "Order notification sent successfully." });
        //}


        //[HttpPost("notify-staff/paid")]
        //public async Task<IActionResult> NotifyStaffNewPaid(int restaurantId, [FromBody] TestNewPaid request)
        //{

        //    // Gửi thông báo đến staff của nhà hàng
        //    await _orderDetailHubContext.Clients.Group("Order" + request.orderId.ToString()).SendAsync("PaidComplete", request.paymentId.ToString());
        //    return Ok(new { Message = "Order notification sent successfully." });
        //}

      
    }

    public class TestNewPaid
    {
        public int orderId { get; set; }
        public int paymentId { get; set; }
    }
}
