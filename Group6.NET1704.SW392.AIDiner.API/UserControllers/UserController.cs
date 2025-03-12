using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.Common.Model.UserModel;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FS.HotelBooking.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ResponseDTO> GetUserFromLogin()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                return new ResponseDTO
                {
                    IsSucess = false,
                    Data = null,
                    BusinessCode = BusinessCode.NOT_FOUND,
                };
            }

            return await _service.GetUserById(parsedUserId);
        }

        [HttpPut("update-profile")]
        public async Task<ResponseDTO> UpdateProfile([FromBody] UpdateProfileUserModel userDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                return new ResponseDTO
                {
                    IsSucess = false,
                    Data = null,
                    BusinessCode = BusinessCode.NOT_FOUND,
                };
            }

            return await _service.UpdateProfileUser(parsedUserId, userDTO);

        }
    }
}
