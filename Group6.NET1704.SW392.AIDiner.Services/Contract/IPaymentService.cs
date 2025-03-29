using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.Contract
{
    public interface IPaymentService
    {
        Task<ResponseDTO> GetPayments(int? restaurantId);
        Task<ResponseDTO> GetPaymentByRestaurantId(int? restaurantId);
        Task<ResponseDTO> PaidByCash(PaidByCashRequest request);
        Task<ResponseDTO> GetPaymentInfoById(int paymentId);

    }

}
