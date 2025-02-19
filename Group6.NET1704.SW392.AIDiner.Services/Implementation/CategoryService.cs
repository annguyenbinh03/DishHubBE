using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Group6.NET1704.SW392.AIDiner.DAL;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using System.Linq.Expressions;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.Common.DTO.Request;

namespace Group6.NET1704.SW392.AIDiner.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private IGenericRepository<Category> _categoryRepositoy;
        private IUnitOfWork _unitOfWork;

        public CategoryService(IGenericRepository<Category> categoryRepositoy, IUnitOfWork unitOfWork)
        {
            _categoryRepositoy = categoryRepositoy;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> GetAllCategory()
        {
           ResponseDTO dto = new ResponseDTO();
            try
            {
                var category = await _categoryRepositoy.GetAllDataByExpression(null, 0, 0, null, true);
                dto.Data = category.Items.Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,                   
                    //Status = c.Status,
                    Image = c.Image,
                }).ToList();
                dto.IsSucess = true;


            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
               
            }
            return dto;
        }
    }
}
