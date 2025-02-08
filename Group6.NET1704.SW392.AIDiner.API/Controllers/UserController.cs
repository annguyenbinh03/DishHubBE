using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FS.HotelBooking.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ResponseDTO> GetAllUser()
        {
            return await _service.GetAllUser();
        }

        [HttpGet("{id}")]
        public async Task<ResponseDTO> GetUserById(int id)
        {
            return await _service.GetUserById(id);
        }
    }
}
