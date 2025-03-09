using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.Response;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Group6.NET1704.SW392.AIDiner.Services.Implementation;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Group6.NET1704.SW392.AIDiner.Services.Hubs
{
    public class OrderDetailHub : Hub
    {

        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailHub(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext.Request.Query.TryGetValue("restaurantId", out var restaurantId))
            {
                if (int.TryParse(restaurantId, out int parsedRestaurantId))
                {
                    // Thêm Staff vào nhóm theo RestaurantId
                    await Groups.AddToGroupAsync(Context.ConnectionId, parsedRestaurantId.ToString());

                    // Gửi danh sách đơn hàng hiện tại
                    var orders = await _orderDetailService.getRestaurantCurrentOrderDetail(parsedRestaurantId);
                    await Clients.Caller.SendAsync("LoadCurrentOrders", orders);
                }
            }
            await base.OnConnectedAsync();
        }

        //Khi khách hàng đặt món, đẩy lên danh sách và thông báo staff
        public async Task NotifyStaffNewOrder(int restaurantId, OrderDetailHubResponse orderDetail)
        {

            await Clients.Group(restaurantId.ToString())
               .SendAsync("ReceiveNewOrder", orderDetail);
        }
    }
}
