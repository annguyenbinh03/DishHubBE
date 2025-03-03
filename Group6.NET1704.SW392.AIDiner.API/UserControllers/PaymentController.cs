using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Services.PaymentGateWay;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            Dictionary<string, string> queryParams = HttpContext.Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());
            ResponseDTO response = await _vpnpayService.ConfirmPayment(queryParams);

            if (response.IsSucess)
            {
                // Lấy URL Redirect từ Response
                string redirectUrl = response.Data.GetType().GetProperty("RedirectUrl")?.GetValue(response.Data, null)?.ToString();
                return Redirect(redirectUrl);
            }

            return BadRequest(response);
        }

    }
}
