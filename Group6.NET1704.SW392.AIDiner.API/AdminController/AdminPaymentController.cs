using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Group6.NET1704.SW392.AIDiner.API.AdminController
{
    [Route("api/admin/payments")]
    [ApiController]
    [Authorize(Roles = "Manager")]
    public class AdminPaymentController : ControllerBase
    {
        private IPaymentService _paymentService;

        public AdminPaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

      



    }
}
