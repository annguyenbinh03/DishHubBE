using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.Common.DTO.Request;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.Implementation
{
    public class DishService : IDishService
    {
        private IGenericRepository<Dish> _dishRepository;
        private IUnitOfWork _unitOfWork;

        public DishService(IGenericRepository<Dish> dishRepository, IUnitOfWork unitOfWork)
        {
            _dishRepository = dishRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> GetAllDishes()
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var dishes = await _dishRepository.GetAllDataByExpression(null, 0, 0, null, true, d => d.Category);
                dto.Data = dishes.Items.Select(d => new DishDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    CategoryId = d.CategoryId,
                   Price = d.Price,
                   Image = d.Image,
                   //Status = d.Status,
                }).ToList();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                dto.IsSucess= false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        public async Task<ResponseDTO> GetDishByIdAsync(int dishId)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var dish = await _dishRepository.GetById(dishId);
                if (dish == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.NOT_FOUND;
                    return dto;
                }
                dto.Data = new DishDTO
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Description = dish.Description,
                    CategoryId = dish.CategoryId,
                    Price = dish.Price,
                    Image = dish.Image,
                    //Status = dish.Status,
                };
                dto.IsSucess = true;


            }
            catch (Exception ex) 
            { 
                dto.IsSucess= false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }
    }
}
