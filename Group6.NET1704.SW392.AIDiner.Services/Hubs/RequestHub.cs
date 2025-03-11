using Group6.NET1704.SW392.AIDiner.Common.Response;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Group6.NET1704.SW392.AIDiner.Services.Implementation;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.Hubs
{
    public class RequestHub : Hub
    {

        private readonly IRequestService _requestService;

        public RequestHub(IRequestService requestService) { 
            _requestService = requestService;
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
                    var requests = await _requestService.GetAllRequest(parsedRestaurantId);
                    await Clients.Caller.SendAsync("LoadCurrentRequest", requests);
                }
            }
            await base.OnConnectedAsync();
        }

        //Khi khách hàng đặt món, đẩy lên danh sách và thông báo staff
        public async Task NotifyStaffNewRequest(int restaurantId, RequestHubResponse request)
        {

            await Clients.Group(restaurantId.ToString())
               .SendAsync("ReceiveNewRequest", request);
        }
    }
}
