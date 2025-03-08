using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.Common.DTO;
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
    public class PaymentService : IPaymentService
    {
        private readonly IGenericRepository<Payment> _paymentRepository;
        private readonly IGenericRepository<PaymentMethod> _paymentMethodRepository;
        private readonly IGenericRepository<Order> _orderRepository;

        public PaymentService(
            IGenericRepository<Payment> paymentRepository,
            IGenericRepository<PaymentMethod> paymentMethodRepository,
            IGenericRepository<Order> orderRepository)
        {
            _paymentRepository = paymentRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _orderRepository = orderRepository;
        }

        public async Task<ResponseDTO> GetPayments(int? restaurantId)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var payments = await _paymentRepository.GetAllDataByExpression(
                    p => restaurantId == null || p.Order.Table.RestaurantId == restaurantId,
                    0, 0, null, true, p => p.Method, p => p.Order.Table);

                dto.Data = payments.Items.Select(p => new PaymentDTO
                {
                    Id = p.Id,
                    OrderId = p.OrderId,
                    MethodId = p.MethodId,
                    MethodName = p.Method.Name,
                    TransactionCode = p.TransactionCode,
                    CreatedAt = p.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
                    Amount = p.Amount,
                    Description = p.Description,
                    Status = p.Status
                }).ToList();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.message = "Get Payment from restaurant successfully";

            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Data = ex.Message;
            }
            return dto;
        }

        public async Task<ResponseDTO> GetPaymentByRestaurantId(int? restaurantId)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var payments = await _paymentRepository.GetAllDataByExpression(
                    p => !restaurantId.HasValue || p.Order.Table.RestaurantId == restaurantId.Value,
                    0, 0, null, true, p => p.Method, p => p.Order.Table);

                dto.Data = payments.Items.Select(p => new PaymentDTO
                {
                    Id = p.Id,
                    OrderId = p.OrderId,
                    MethodId = p.MethodId,
                    MethodName = p.Method?.Name ?? "Unknown",
                    TransactionCode = p.TransactionCode,
                    CreatedAt = p.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
                    Amount = p.Amount,
                    Description = p.Description,
                    Status = p.Status
                }).ToList();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.message = "Get payments successfully";
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
