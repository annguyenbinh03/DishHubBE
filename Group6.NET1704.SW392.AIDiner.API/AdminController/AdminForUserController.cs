using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.Common.UserModel;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
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

        [HttpGet]
        public async Task<ResponseDTO> GetAllUser(int pageNumber, int pageSize)
        {
            return await _service.GetAllUser(pageNumber, pageSize);
        }


        [HttpGet("{id}")]
        public async Task<ResponseDTO> GetUserById(int id)
        {
            return await _service.GetUserById(id);
        }

        [HttpPut("admin/users")]
        [Authorize(Roles = "User")]
        public async Task<ResponseDTO> UpdateUser([FromBody] UpdateUserModel userDTO)
        {
            return await _service.UpdateUserForAdmin(userDTO);
        }
    }
}
