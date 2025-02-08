using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Implementation;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.Implementation
{
    public class IngredientService : IIngredientService
    {
        private IUnitOfWork _unitOfWork;
        private IGenericRepository<Ingredient> _ingredientRepository;

        public IngredientService(IUnitOfWork unitOfWork, IGenericRepository<Ingredient> ingredientRepository)
        {
            _unitOfWork = unitOfWork;
            _ingredientRepository = ingredientRepository;
        }

        public async Task<ResponseDTO> GetAllIngredients()
        {
           ResponseDTO dto = new ResponseDTO();
            try
            {
                var ingredients = await _ingredientRepository.GetAllDataByExpression(null, 0 , 0 , null, true);
                dto.Data = ingredients.Items.Select(a => new IngredientDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    Image = a.Image,
                    DishCount = a.DishIngredients.Count // Đếm số món ăn sử dụng nguyên liệu này
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
