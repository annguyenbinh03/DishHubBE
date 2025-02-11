using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.Common.DTO.Request;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.Implementation
{
    public class TableService : ITableService
    {
        private IGenericRepository<Table> _tableRepository;
        private IUnitOfWork _unitOfWork;

        public TableService(IGenericRepository<Table> tableRepository, IUnitOfWork unitOfWork)
        {
            _tableRepository = tableRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> GetAllTable()
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var tables = await _tableRepository.GetAllDataByExpression(null, 0, 0, null, true);
                dto.Data = tables.Items.Select(d => new TableDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Qrcode = d.Qrcode,
                    CreatedAt = d.CreatedAt,
                    Status = d.Status,
                }).ToList();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }
    }
}
