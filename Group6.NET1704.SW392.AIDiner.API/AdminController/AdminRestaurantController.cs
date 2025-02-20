using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Group6.NET1704.SW392.AIDiner.API.AdminController
{
    [Route("api/admin/restaurants")]
    [ApiController]
    public class AdminRestaurantController : ControllerBase
    {
        private IRestaurantService _service;
        public AdminRestaurantController(IRestaurantService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var response = await _service.GetAll();
            return Ok(response);
        }
    }
}
