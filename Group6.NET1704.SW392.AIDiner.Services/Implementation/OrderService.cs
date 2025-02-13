using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
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
    public class OrderService : IOrderService
    {

        private IGenericRepository<Order> _orderRepository;
        private IUnitOfWork _unitOfWork;

        public OrderService(IGenericRepository<Order> orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<ResponseDTO> GetAllOrder()
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var orders = await _orderRepository.GetAllDataByExpression(null, 0, 0, null, true,
                     includes: new Expression<Func<Order, object>>[] { b => b.Customer, b => b.Table });
                dto.Data = orders.Items.Select(x => new OrderDTO
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    TableId = x.TableId,
                    TotalAmount = x.TotalAmount,
                    PaymentStatus = x.PaymentStatus,
                    CreatedAt = x.CreatedAt,
                    Status = x.Status,
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

        public async Task<ResponseDTO> GetByOrderId(int id)
        {
          
                ResponseDTO dto = new ResponseDTO();
                try
                {
                    var order = await _orderRepository.GetById(id);

                    if (order == null)
                    {
                        dto.IsSucess = false;
                        return dto;

                    }

                    dto.Data = new OrderDTO
                    {
                        Id=order.Id,
                        CustomerId=order.CustomerId,
                        TableId=order.TableId,
                        TotalAmount = order.TotalAmount,
                        PaymentStatus = order.PaymentStatus,
                        CreatedAt = order.CreatedAt,
                        Status = order.Status,
                    };

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

