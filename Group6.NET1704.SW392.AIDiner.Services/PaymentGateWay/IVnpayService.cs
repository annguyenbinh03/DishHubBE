using Group6.NET1704.SW392.AIDiner.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.PaymentGateWay
{
    public interface IVnpayService
    {
        Task<ResponseDTO> Charge(int orderId, int methodId);
        string CreatePaymentUrl(decimal amount, int orderId);
        Task<ResponseDTO> ConfirmPayment(Dictionary<string, string> queryParams);
    }
}
