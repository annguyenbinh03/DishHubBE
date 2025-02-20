using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.Implementation
{
    public class RequestService : IRequestService
    {
        private IGenericRepository<Request> _requestRepository;
        private IGenericRepository<Order> _orderRepository;
        private IGenericRepository<RequestType> _requestTypeRepository;


        private IUnitOfWork _unitOfWork;

        public RequestService(IGenericRepository<Request> requestRepository, IGenericRepository<Order> orderRepository, IGenericRepository<RequestType> requestTypeRepository, IUnitOfWork unitOfWork)
        {
            _requestRepository = requestRepository;
            _orderRepository = orderRepository;
            _requestTypeRepository = requestTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> GetRequestByOrderID(int orderID)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var requests = await _requestRepository.GetAllDataByExpression(
                    filter: r => r.OrderId == orderID, 0, 0,
                    includes: new Expression<Func<Request, object>>[]
                    {
                        r => r.Type
                    });

                if (!requests.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.message = "No request found for this order ID";
                    return dto;
                }

                dto.Data = requests.Items.Select(request => new RequestDTO
                {
                    Id = request.Id,
                    TypeId = request.TypeId,
                    CreatedAt = request.CreatedAt,
                    Note = request.Note,
                    Status = request.Status

                }).ToList();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.message = "Get requests by order ID successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.message = "An error occurred: " + ex.Message;
            }
            return dto;
        }


        public async Task<ResponseDTO> CreateRequest(CreateRequestDTO requestDto)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                if (requestDto == null || requestDto.OrderId <= 0 || requestDto.TypeId <= 0)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.INVALID_INPUT;
                    dto.message = "Invalid input parameters";
                    return dto;
                }

                // Kiểm tra OrderId và TypeId có tồn tại không
                var existingOrder = await _orderRepository.GetById(requestDto.OrderId);
                if (existingOrder == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.NOT_FOUND;
                    dto.message = "Order not found";
                    return dto;
                }

                var existingType = await _requestTypeRepository.GetById(requestDto.TypeId);
                if (existingType == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.NOT_FOUND;
                    dto.message = "Request type not found";
                    return dto;
                }

                Request newRequest = new Request
                {
                    OrderId = requestDto.OrderId,
                    TypeId = requestDto.TypeId,
                    Note = requestDto.Note,
                    CreatedAt = DateTime.UtcNow,
                    Status = "pending"
                };

                await _requestRepository.Insert(newRequest);
                await _unitOfWork.SaveChangeAsync(); // Chỉ gọi 1 hàm lưu dữ liệu

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.CREATE_SUCCESS;
                dto.message = "Request created successfully";
                dto.Data = new RequestDTO
                {
                    Id = newRequest.Id,
                    TypeId = newRequest.TypeId,
                    CreatedAt = newRequest.CreatedAt,
                    Note = newRequest.Note,
                    Status = newRequest.Status
                };
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.message = $"An error occurred while creating request: {ex.Message} {ex.InnerException?.Message}";
            }
            return dto;
        }

    }
}
