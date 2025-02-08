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
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("get-all-user")]
        public async Task<ResponseDTO> GetAllUser()
        {
            return await _service.GetAllUser();
        }    
    }
}
