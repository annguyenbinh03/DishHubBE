using Group6.NET1704.SW392.AIDiner.Common.DTO.AdminDTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Group6.NET1704.SW392.AIDiner.API.AdminController
{
    [Route("api/admin")]
    [ApiController]
    public class AdminCategoryController : ControllerBase
    {
        private ICategoryService _categoryService;

        public AdminCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("dish-categories")]

        public async Task<ResponseDTO> CreateCategoryForAdmin(CreateDishCategoryForAdminDTO createDishCategoryForAdminDTO)
        {
            return await _categoryService.CreateCategoryForAdmin(createDishCategoryForAdminDTO);
        }

    }
}
