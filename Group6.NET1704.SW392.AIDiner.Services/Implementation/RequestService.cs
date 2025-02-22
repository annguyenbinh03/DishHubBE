using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.EntityFrameworkCore;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<ResponseDTO> GetAllRequest()
        {
           ResponseDTO response = new ResponseDTO();
            try
            {
                var requests = await _requestRepository.GetQueryable().Include(r => r.Type)
                    .Select(r => new
                    {
                        r.Id,
                        r.OrderId,
                        r.TypeId,
                        TypeName = r.Type.Name,
                        r.Note,
                        CreatedAt = r.CreatedAt.HasValue ? r.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                        ProcessedAt = r.ProcessedAt.HasValue ? r.ProcessedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                        r.Status,
                    }).ToListAsync();
                response.IsSucess = true;
                response.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                response.Data = requests;
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.BusinessCode = BusinessCode.EXCEPTION;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> UpdateRequestStatus(int requestId, string status)
        {
            ResponseDTO response = new ResponseDTO();
            try
            {
                var request = await _requestRepository.GetQueryable().Include(r => r.Order).Include(r => r.Type).FirstOrDefaultAsync(r => r.Id == requestId);
                if (request == null)
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.NOT_FOUND;
                    response.Data = "Request not found";
                    return response;
                }
                var allowedStatuses = new[] { "pending", "inProgress", "completed", "cancelled" };
                if(!Array.Exists(allowedStatuses, s => s.Equals(status, StringComparison.OrdinalIgnoreCase)))
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.INVALID_INPUT;
                    response.Data = "Invalid status value";
                    return response;
                }
                request.Status = status;
                request.ProcessedAt = DateTime.UtcNow;
                
                await _requestRepository.Update(request);
                await _unitOfWork.SaveChangeAsync();
                
                response.IsSucess = true;
                response.BusinessCode = BusinessCode.UPDATE_SUCESSFULLY;
                response.Data = new
                {
                    request.Id,
                    request.OrderId,
                    request.TypeId,
                    request.CreatedAt,
                    request.ProcessedAt,
                    request.Note,
                    request.Status
                };
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.BusinessCode = Common.DTO.BusinessCode.BusinessCode.EXCEPTION;
                response.Data = ex.Message;
            }
            return response;
        }
    }
}
