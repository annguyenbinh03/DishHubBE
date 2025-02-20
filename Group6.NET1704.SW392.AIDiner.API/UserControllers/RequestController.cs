using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Group6.NET1704.SW392.AIDiner.API.UserControllers
{
    [Route("api/requests/history")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private IRequestService _requestService;

        public RequestController(IRequestService requestService)
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
