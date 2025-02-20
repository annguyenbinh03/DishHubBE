using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.Common.Request;
using Group6.NET1704.SW392.AIDiner.Common.Response;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
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

        public RestaurantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
