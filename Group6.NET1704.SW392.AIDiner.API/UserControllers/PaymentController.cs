using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.Services.PaymentGateWay;
using Group6.NET1704.SW392.AIDiner.Services.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Group6.NET1704.SW392.AIDiner.API.UserControllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private IVnpayService _vpnpayService;

        public PaymentController(IVnpayService vpnpayService)
        {
            _vpnpayService = vpnpayService;
        }
        [HttpPost("{orderId}/pay")]
        public async Task<ResponseDTO> ProcessPayment(int orderId)
        {
            int methodId = 1;
            return await _vpnpayService.Charge(orderId, methodId);
        }
        [HttpGet("vnpay/confirm")]
        public async Task<IActionResult> ConfirmPayment()
        {
            ResponseDTO response = await _vpnpayService.ConfirmPayment(Request);
            if (response.IsSucess)
            {
                string redirectUrl = response.Data?.GetType().GetProperty("redirectURL")?.GetValue(response.Data, null)?.ToString() ?? "/default-url";
                return Redirect(redirectUrl);
            }
            else
            {
                return Ok(response);
            }
        }
    }
}
