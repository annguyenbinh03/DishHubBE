using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Group6.NET1704.SW392.AIDiner.API.AdminController
{
    [Route("api/admin")]
    [ApiController]
    public class AdminForOrderController : ControllerBase
    {
        private IOrderService _orderService;

        public AdminForOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("orders")]
        public async Task<ResponseDTO> GetAllOrder()
        {
            return await _orderService.GetAllOrder();
        }

      
    }
}
