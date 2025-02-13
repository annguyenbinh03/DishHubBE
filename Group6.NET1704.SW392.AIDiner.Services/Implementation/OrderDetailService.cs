using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IGenericRepository<OrderDetail> _orderDetailRepositoy;
        private readonly IGenericRepository<Order> _orderRepository; // ✅ Thêm Order Repository
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailService(IGenericRepository<OrderDetail> orderDetailRepositoy,
                                  IGenericRepository<Order> orderRepository, // ✅ Inject Order Repository
                                  IUnitOfWork unitOfWork)
        {
            _orderDetailRepositoy = orderDetailRepositoy;
            _orderRepository = orderRepository; // ✅ Gán repository mới
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> GetOrderDetailByOrderID(int orderId)
        {
            ResponseDTO dto = new ResponseDTO();

            try
            {
                if (orderId <= 0)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.INVALID_INPUT;
                    return dto;
                }

                // Lấy thông tin Order từ repository
                var order = await _orderRepository.GetByExpression(o => o.Id == orderId);

                if (order == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.NOT_FOUND;
                    return dto;
                }

                // Lấy danh sách OrderDetail theo OrderId
                var orderDetails = await _orderDetailRepositoy.GetAllDataByExpression(
                    od => od.OrderId == orderId, 0, 0
                );

                // Nếu không có OrderDetail
                if (orderDetails == null || orderDetails.Items.Count == 0)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.NOT_FOUND;
                    return dto;
                }

                // Gộp thông tin Order và danh sách OrderDetail vào response
                dto.Data = new
                {
                    Order = new
                    {
                        order.Id,
                        order.CustomerId,
                        order.TableId,
                        order.TotalAmount,
                        order.PaymentStatus,
                        order.CreatedAt,
                        order.Status,
                        OrderDetails = orderDetails.Items.Select(orderdetail => new
                        {
                            orderdetail.DishId,
                            orderdetail.Quantity,
                            orderdetail.Price,
                            orderdetail.Status
                        }).ToList()
                    }
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
