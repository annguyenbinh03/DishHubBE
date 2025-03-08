using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.AdminDTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.Common.UserModel;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Group6.NET1704.SW392.AIDiner.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Group6.NET1704.SW392.AIDiner.API.AdminController
{
    [Route("api/admin")]
    [ApiController]
    public class AdminForUserController : ControllerBase
    {
        private IUserService _service;

        public AdminForUserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("users")]
        public async Task<ResponseDTO> GetAllUser(int page = 1, int size = 10, string? search = null, string? sort_by = null, string? sort_order = null)
        {
            return await _service.GetAllUser(page, size, search, sort_by, sort_order);
        }


        [HttpGet("users/{id}")]
        public async Task<ResponseDTO> GetUserById(int id)
        {
            return await _service.GetUserById(id);
        }

        [HttpPut("users/{id}")]
        public async Task<ResponseDTO> UpdateUser(int id, [FromBody] UpdateUserModel userDTO)
        {
            return await _service.UpdateUserForAdmin(id, userDTO);
        }

        [HttpDelete("users/{id}")]
        public async Task<ResponseIsSucessDTO> DeleteUserForAdmin(int id)
        {
            return await _service.DeleteUserForAdmin(id);

        }
        [HttpPost("users")]
        public async Task<ResponseDTO> CreateUser([FromBody] AdminCreateAccountDTO model)
        {
            return await _service.CreateUserAsync(model);
        }
    }
}
