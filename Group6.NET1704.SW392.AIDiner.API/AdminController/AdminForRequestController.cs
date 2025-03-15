using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Group6.NET1704.SW392.AIDiner.API.AdminController
{
    [Route("api/requests/history")]
    [ApiController]
    [Authorize]
    public class AdminForRequestController : ControllerBase
    {
        private IRequestService _requestService;

        public AdminForRequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }
        [HttpGet]
        public async Task<ResponseDTO> GetRequestByOrderID(int orderID)
        {
            return await _requestService.GetRequestByOrderID(orderID);
        }

      
    }
}
