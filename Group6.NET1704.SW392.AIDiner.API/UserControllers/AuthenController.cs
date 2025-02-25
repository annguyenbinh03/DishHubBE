using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.Request;
using Group6.NET1704.SW392.AIDiner.Common.Model.RegisterLoginModel;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Group6.NET1704.SW392.AIDiner.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IAuthenService _authenService;

        public AuthenController(IAuthenService authenService)
        {
            _authenService = authenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (model == null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Username and password are required.");
            }
            var token = await _authenService.Login(model);
            if (string.IsNullOrEmpty((string?)token))
            {
                return Unauthorized(new { code = 401, message = "Invalid username or password" });
            }

            return Ok(new { Token = token, message = "Login successful" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    return BadRequest("Username and password are required.");
                }

                var result = await _authenService.Register(model);
                if (result.StartsWith("Internal server error"))
                {
                    return StatusCode(500, result);
                }
                else if (result == "Email already exists" || result == "User already exists" || result == "Phone number already exists")
                {
                    return BadRequest(result);
                }

                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var result = await _authenService.Logout(token);
            if (result)
            {
                return Ok(new { message = "Logout successful" });
            }

            return BadRequest(new { message = "Logout failed" });
        }

        [HttpGet("user-info")]
        [Authorize] 
        public IActionResult GetUserInfo()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return Unauthorized(new { message = "Token không hợp lệ hoặc đã hết hạn" });
            }

            var claims = identity.Claims.ToList();

            var userInfo = new
            {
                UserId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                UserName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                RoleId = claims.FirstOrDefault(c => c.Type == "RoleId")?.Value
            };

            return Ok(userInfo);
        }

        [HttpPost("login-google")]
        public async Task<IActionResult> LoginGoogle([FromBody] GoogleAuthRequest model)
        {
            var response = await _authenService.LoginGoogle(model);

            return Ok(response);
        }
    }
}