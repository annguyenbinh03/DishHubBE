using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.Common.Request;
using Group6.NET1704.SW392.AIDiner.Common.Response;
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
    public class RestaurantService : IRestaurantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Restaurant> _restaurantRepository;

        public RestaurantService(IUnitOfWork unitOfWork, IGenericRepository<Restaurant> restaurantRepository)
        {
            _unitOfWork = unitOfWork;
            _restaurantRepository = restaurantRepository;
        }

        public async Task<ResponseDTO> Create(RestaurantCreationRequest request)
        {
            ResponseDTO response = new ResponseDTO();
            try
            {
                response.Data = await _unitOfWork.Restaurants.Create(request);
                response.IsSucess = true;
                response.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.BusinessCode = BusinessCode.EXCEPTION;
            }
            return response;
        }

        public async Task<ResponseDTO> Delete(int id)
        {
            ResponseDTO response = new ResponseDTO();
            try
            {
                var restaurant = await _restaurantRepository.GetById(id);
                if (restaurant == null)
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.NOT_FOUND;
                    response.message = "Restaurant not found";
                    return response;
                }

                restaurant.IsDeleted = true;
                await _restaurantRepository.Update(restaurant);
                await _unitOfWork.SaveChangeAsync();

                response.IsSucess = true;
                response.BusinessCode = BusinessCode.DELETE_SUCCESS;
                response.message = "Restaurant deleted successfully";
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.BusinessCode = BusinessCode.EXCEPTION;
                response.message = "Error: " + ex.Message;
            }
            return response;
        }
    

        public async Task<ResponseDTO> GetAll()
        {
            ResponseDTO response = new ResponseDTO();
            try
            {
                response.Data = await _unitOfWork.Restaurants.GetAll();
                response.IsSucess = true;
                response.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.BusinessCode = BusinessCode.EXCEPTION;
            }
            return response;
        }

        public async Task<ResponseDTO> GetAllWithTablesAsync()
        {

            ResponseDTO response = new ResponseDTO();
            try
            {
                response.Data = await _unitOfWork.Restaurants.GetAllWithTablesAsync();
                response.IsSucess = true;
                response.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.BusinessCode = BusinessCode.EXCEPTION;
            }
            return response;
        }

        public async Task<ResponseDTO> Update(int id, RestaurantUpdateRequest request)
        {
            ResponseDTO response = new ResponseDTO();
            try
            {
                response.Data = await _unitOfWork.Restaurants.Update(id, request);
                response.IsSucess = true;
                response.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.BusinessCode = BusinessCode.EXCEPTION;
            }
            return response;
        }
    }   
}
