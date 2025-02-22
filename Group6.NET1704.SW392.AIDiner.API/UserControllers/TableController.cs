using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Group6.NET1704.SW392.AIDiner.API.UserControllers
{
    [Route("api/table")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private ITableService _service;

        public TableController(ITableService service)
        {
            _service = service;
        }

        //[HttpGet]
        //public async Task<ResponseDTO> GetAllTable()
        //{
        //    return await _service.GetAllTable();
        //}
    }
}
