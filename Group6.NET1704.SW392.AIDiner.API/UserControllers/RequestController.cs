using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.Request;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Group6.NET1704.SW392.AIDiner.API.UserControllers
{
    [Route("api/request")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private IRequestService _requestService;
        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }
        [HttpPatch("{id}")]
       // [Authorize(Roles = "Staff")]
        public async Task<ResponseDTO> UpdateRequestStatus(int id, [FromBody] UpdateRequestDTO updateRequestDTO)
        {
            return await _requestService.UpdateRequestStatus(id, updateRequestDTO.Status);
        }
        // [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<ResponseDTO> GetAllRequest()
        {
            return await _requestService.GetAllRequest();
        }
    }
}
