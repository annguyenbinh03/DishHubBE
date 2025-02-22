using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.AdminDTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.Common.DTO.Request;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Data;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Group6.NET1704.SW392.AIDiner.Services.Implementation
{
    public class DishService : IDishService
    {
        private IGenericRepository<Dish> _dishRepository;
        private IUnitOfWork _unitOfWork;
        private IGenericRepository<Ingredient> _ingredientRepository;

        public DishService(IGenericRepository<Dish> dishRepository, IUnitOfWork unitOfWork, IGenericRepository<Ingredient> ingredientRepository)
        {
            _dishRepository = dishRepository;
            _unitOfWork = unitOfWork;
            _ingredientRepository = ingredientRepository;
        }

        public async Task<ResponseDTO> CreateDish(CreateDishDTO createDishDTO)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var allowedStatuses = new List<string> { "onsale", "inactive" };
                if (string.IsNullOrEmpty(createDishDTO.Status) || !allowedStatuses.Contains(createDishDTO.Status.ToLower()))
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.INVALID_INPUT;
                    dto.Data = "Invalid status value. Allowed values: 'onsale', 'inactive'.";
                    return dto;
                }

                var newDish = new Dish
                {
                    Name = createDishDTO.Name,
                    Description = createDishDTO.Description,
                    CategoryId = createDishDTO.CategoryId,
                    Price = createDishDTO.Price,
                    Image = createDishDTO.Image,
                    Status = createDishDTO.Status.ToLower(),
                    RestaurantId = createDishDTO.RestaurantId,
                    DishIngredients = new List<DishIngredient>()

                };
                foreach (var ingredientId in createDishDTO.Ingredients)
                {
                    newDish.DishIngredients.Add(new DishIngredient
                    {
                        IngredientId = ingredientId
                    });
                }
                await _dishRepository.Insert(newDish);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.CREATE_SUCCESS;
                dto.Data = new { newDish.Id, newDish.Name };
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Data = ex.InnerException?.Message ?? ex.Message;
            }
            return dto;
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
                    RestaurantId = d.RestaurantId,
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

        public async Task<ResponseDTO> GetDishByIdAsync(int dishId)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var dishes = await _dishRepository.GetDishesWithIngredients(d => d.Id == dishId);
                if (dishes.Count == 0)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.NOT_FOUND;
                    return dto;
                }
                var dishData = dishes.First();
                dto.Data = new DishDetailDTO
                {
                    Id = dishData.Id,
                    Name = dishData.Name,
                    Description = dishData.Description,
                    CategoryId = dishData.CategoryId,
                    Price = dishData.Price,
                    Image = dishData.Image,
                    SoldCount = dishData.SoldCount,
                    Ingredients = dishData.DishIngredients?.Select(di => new IngredientDTO
                    {
                        Id = di.Ingredient.Id,
                        Name = di.Ingredient.Name,
                        Image = di.Ingredient.Image
                    }).ToList()
                };

                dto.IsSucess = true;
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Data = ex.Message;
            }
            return dto;
        }

        public async Task<ResponseDTO> GetDishesForAdmin(int? categoryId, int page, int size, string? search, string? sortBy, string? sortOrder)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var query = _dishRepository
                    .GetQueryable()
                    .Include(d => d.Category)
                    .Include(d => d.DishIngredients)
                        .ThenInclude(di => di.Ingredient) 
                    .AsQueryable();

                if (categoryId.HasValue)
                {
                    query = query.Where(d => d.CategoryId == categoryId.Value);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d => EF.Functions.Like(d.Name, $"%{search}%"));
                }

                query = sortBy?.ToLower() switch
                {
                    "name" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name),
                    "price" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(d => d.Price) : query.OrderBy(d => d.Price),
                    "soldcount" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(d => d.SoldCount) : query.OrderBy(d => d.SoldCount),
                    _ => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(d => d.Id) : query.OrderBy(d => d.Id)
                };

                int totalItems = await query.CountAsync();
                int totalPages = (int)Math.Ceiling(totalItems / (double)size);

                var dishes = await query
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToListAsync();

                dto.Data = new
                {
                    TotalPages = totalPages,
                    CurrentPage = page,
                    PageSize = size,
                    Dishes = dishes.Select(d => new DishAdminDTO
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Description = d.Description,
                        CategoryId = d.CategoryId,
                        Price = d.Price,
                        Image = d.Image,
                        SoldCount = d.SoldCount,
                        Status = d.Status,
                        Ingredients = d.DishIngredients
                            .Where(di => di.Ingredient != null) 
                            .Select(di => new IngredientDTO
                            {
                                Id = di.Ingredient!.Id,
                                Name = di.Ingredient!.Name,
                                Image = di.Ingredient!.Image
                            }).ToList()
                    }).ToList()
                };

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Data = ex.Message;
            }
            return dto;
        }


        public async Task<ResponseDTO> UpdateDish(int dishId, UpdateDishDTO updateDishDTO)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var dish = await _dishRepository
                    .GetQueryable()
                    .Include(d => d.DishIngredients) //  Load danh sách nguyên liệu hiện tại
                    .FirstOrDefaultAsync(d => d.Id == dishId);

                if (dish == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.NOT_FOUND;
                    dto.Data = "Dish not found.";
                    return dto;
                }

                // Cập nhật thông tin món ăn
                dish.Name = updateDishDTO.Name ?? dish.Name;
                dish.Description = updateDishDTO.Description ?? dish.Description;
                dish.CategoryId = updateDishDTO.CategoryId ?? dish.CategoryId;
                dish.Price = updateDishDTO.Price ?? dish.Price;
                dish.Image = updateDishDTO.Image ?? dish.Image;
                dish.Status = updateDishDTO.Status ?? dish.Status;

                //  Chỉ xóa quan hệ trong DishIngredient, không xóa nguyên liệu thật sự
                await _unitOfWork.DishIngredientRepository.DeleteWhere(di => di.DishId == dishId);
                await _unitOfWork.SaveChangeAsync();

                if (updateDishDTO.Ingredients != null && updateDishDTO.Ingredients.Any())
                {
                    var newIngredients = updateDishDTO.Ingredients
                        .Select(ingredientId => new DishIngredient
                        {
                            DishId = dishId,
                            IngredientId = ingredientId  // Giữ nguyên Ingredient, chỉ cập nhật quan hệ
                        }).ToList();

                    await _unitOfWork.DishIngredientRepository.InsertRange(newIngredients);
                }
                await _dishRepository.Update(dish);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCESSFULLY;
                dto.Data = new { dish.Id, dish.Name };
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Data = ex.Message;
            }
            return dto;
        }

    }
}
