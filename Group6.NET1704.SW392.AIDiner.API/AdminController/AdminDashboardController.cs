using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Group6.NET1704.SW392.AIDiner.API.AdminController
{
    [Route("api/dashboard")]
    [ApiController]
    [Authorize(Roles = "Manager")]
    public class AdminDashboardController
    {
        private readonly IDashboardService _dashboardService;
        public AdminDashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("sales-data/{restaurantId}")]
        public async Task<ResponseDTO> GetSalesData(int restaurantId)
        {
            return await _dashboardService.GetSalesInfo(restaurantId);
        }

        [HttpGet("top-dishes/{restaurantId}")]
        public async Task<ResponseDTO> GetTopDishes(int restaurantId)
        {
            return await _dashboardService.GetTopDishesInfo(restaurantId);
        }

        [HttpGet("tables/{restaurantId}")]
        public async Task<ResponseDTO> GetTables(int restaurantId)
        {
            ResponseDTO response = new ResponseDTO();
             var fakeTables = new List<object>
            {
                new { id = 1, tableName = "Bàn 1", status = "available" },
                new { id = 2, tableName = "Bàn 2", status = "occupied" },
                new { id = 3, tableName = "Bàn 3", status = "available" },
                new { id = 4, tableName = "Bàn 4", status = "occupied" },
                new { id = 5, tableName = "Bàn 5", status = "available" },
                new { id = 6, tableName = "Bàn 6", status = "occupied" },
                new { id = 7, tableName = "Bàn 7", status = "available" },
                new { id = 8, tableName = "Bàn 8", status = "occupied" },
                new { id = 9, tableName = "Bàn 9", status = "available" },
                new { id = 10, tableName = "Bàn 10", status = "occupied" },
                new { id = 11, tableName = "Bàn 11", status = "available" },
                new { id = 12, tableName = "Bàn 12", status = "occupied" },
                new { id = 13, tableName = "Bàn 13", status = "available" },
                new { id = 14, tableName = "Bàn 14", status = "occupied" },
                new { id = 15, tableName = "Bàn 15", status = "available" },
                new { id = 16, tableName = "Bàn 16", status = "occupied" },
                new { id = 17, tableName = "Bàn 17", status = "available" },
                new { id = 18, tableName = "Bàn 18", status = "occupied" },
                new { id = 19, tableName = "Bàn 19", status = "available" },
                new { id = 20, tableName = "Bàn 20", status = "occupied" },
                new { id = 21, tableName = "Bàn 21", status = "available" },
                new { id = 22, tableName = "Bàn 22", status = "occupied" },
                new { id = 23, tableName = "Bàn 23", status = "available" },
             };
            response.IsSucess = true;
            response.Data = fakeTables;
            return response;
        }
    }
}
