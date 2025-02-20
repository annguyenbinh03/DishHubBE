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
        private IUnitOfWork _unitOfWork;

        public RequestService(IGenericRepository<Request> requestRepository, IUnitOfWork unitOfWork)
        {
            _requestRepository = requestRepository;
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
    }
}
