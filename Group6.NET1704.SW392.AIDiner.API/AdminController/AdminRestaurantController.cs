using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.Request;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Group6.NET1704.SW392.AIDiner.API.AdminController
{
    [Route("api/admin/restaurants")]
    [ApiController]
    [Authorize(Roles = "Manager")]
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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RestaurantCreationRequest request)
        {
            var response = await _service.Create(request);
            return Ok(response);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update([FromRoute] int Id,[FromBody] RestaurantUpdateRequest request)
        {
            var response = await _service.Update(Id, request);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<ResponseDTO> Delete([FromRoute] int id)
        {
            return await _service.Delete(id);
        }
            
    }
}
