using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.Services.BusinessObjects;
using Group6.NET1704.SW392.AIDiner.Services.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Web;

namespace Group6.NET1704.SW392.AIDiner.Services.PaymentGateWay
{
    public class VnpayService : IVnpayService
    {
        private readonly VNPaySettings _vNPaySettings;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Payment> _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VnpayService(IOptions<VNPaySettings> vnPaySettings, IGenericRepository<Order> orderRepository, IGenericRepository<Payment> paymentRepository, IUnitOfWork unitOfWork)
        {
            _vNPaySettings = vnPaySettings.Value;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> Charge(int orderId, int methodId)
        {
            ResponseDTO response = new ResponseDTO();
            try
            {
                var order = await _orderRepository.GetById(orderId);
                if (order == null)
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.NOT_FOUND;
                    response.Data = "Order not found";
                    return response;
                }
                if (order.Status.Equals("completed") || order.Status.Equals("cancelled"))
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.NOT_FOUND;
                    response.Data = "Đơn hàng đã hoàn tất hoặc bị hủy!";
                    return response;
                }
                decimal totalAmount = order.TotalAmount;
                if (totalAmount <= 0)
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.INVALID_INPUT;
                    response.Data = "Total amount must be greater than 0";
                    return response;
                }
                string paymentUrl = CreatePaymentUrl(totalAmount, orderId);

                response.IsSucess = true;
                response.BusinessCode = BusinessCode.CREATE_SUCCESS;
                response.Data = new { url = paymentUrl };

            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.BusinessCode = BusinessCode.EXCEPTION;
                response.Data = ex.Message;
            }
            return response;
        }


        public string CreatePaymentUrl(decimal amount, int orderId)
        {
            string hostName = System.Net.Dns.GetHostName();
            string clientIPAddress = System.Net.Dns.GetHostAddresses(hostName).GetValue(0).ToString();
            string infor = "Thanh toan cho orderId: " + orderId.ToString();
            string tnxRef = TimeZoneUtil.GetCurrentTime().ToString("ddHHmmssyyyy");
            tnxRef = tnxRef + orderId.ToString();

            string vnp_Amount = ((int)amount).ToString() + "00";

            VNPayHelper pay = new VNPayHelper();
            pay.AddRequestData("vnp_Version", "2.1.0");
            pay.AddRequestData("vnp_Command", "pay");
            pay.AddRequestData("vnp_TmnCode",_vNPaySettings.TmnCode);
            pay.AddRequestData("vnp_Amount", vnp_Amount);
            pay.AddRequestData("vnp_BankCode", "");
            pay.AddRequestData("vnp_CreateDate", TimeZoneUtil.GetCurrentTime().ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", "VND");
            pay.AddRequestData("vnp_IpAddr", clientIPAddress);
            pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_OrderInfo", orderId.ToString());
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", _vNPaySettings.ReturnUrl);
            pay.AddRequestData("vnp_TxnRef", tnxRef);

            return pay.CreateRequestUrl(_vNPaySettings.VnpayUrl, _vNPaySettings.HashSecret);
        }

        public async Task<ResponseDTO> ConfirmPayment(HttpRequest request)
        {
            ResponseDTO response = new ResponseDTO();
            try
            {
                if (!request.QueryString.HasValue)
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.EXCEPTION;
                    response.message = "VNPAY Querry string not found";
                    return response;
                }

                var queryString = request.QueryString.Value;
                var json = HttpUtility.ParseQueryString(queryString);

                string txnRef = json["vnp_TxnRef"].ToString();
                int orderId = Convert.ToInt32(json["vnp_OrderInfo"]);
                long vnpayTranId = Convert.ToInt64(json["vnp_TransactionNo"]);
                string vnp_ResponseCode = json["vnp_ResponseCode"].ToString();
                string vnp_SecureHash = json["vnp_SecureHash"].ToString();
                string stringAmount = json["vnp_Amount"].ToString();
                int pos = queryString.IndexOf("&vnp_SecureHash");

                bool checkSignature = ValidateSignature(queryString.Substring(1, pos - 1), vnp_SecureHash, _vNPaySettings.HashSecret);
                if (!checkSignature || _vNPaySettings.TmnCode != json["vnp_TmnCode"].ToString())
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.EXCEPTION;
                    response.message = "Invalid signature or incorrect merchant code.";
                    return response;
                }

                decimal amount;
                bool isParseSucess = Decimal.TryParse(stringAmount, out amount);

                if (!isParseSucess)
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.PAYMENT_FAILED;
                    response.Data = "Parse giá trị đơn hàng thất bại";
                    return response;
                }

                amount /= 100;

                if (vnp_ResponseCode == "00") 
                {
                    var payment = new Payment
                    {
                        OrderId = orderId,
                        MethodId = 1,
                        TransactionCode = vnpayTranId.ToString(),
                        CreatedAt = TimeZoneUtil.GetCurrentTime(),
                        Amount = amount,
                        Description = $"Thanh toán thành công cho orderId {orderId}",
                        Status = true,
                    };

                    var order = await _orderRepository.GetById(orderId);

                    await _paymentRepository.Insert(payment);
                    order.Status = "completed";
                    order.PaymentStatus = true;
                    await _orderRepository.Update(order);
                    await _unitOfWork.SaveChangeAsync();

                    string redirectURL = _vNPaySettings.RedirectUrl + $"/{payment.Id}";

                    response.IsSucess = true;
                    response.BusinessCode = BusinessCode.CREATE_SUCCESS;
                    response.Data = new { redirectURL };
                    return response;
                }
                else
                {
                    var payment = new Payment
                    {
                        OrderId = orderId,
                        MethodId = 1,
                        TransactionCode = vnpayTranId.ToString(),
                        CreatedAt = TimeZoneUtil.GetCurrentTime(),
                        Amount = amount,
                        Description = $"Thanh toán thất bại cho orderId {orderId}",
                        Status = false,
                    };
                    await _paymentRepository.Insert(payment);
                    await _unitOfWork.SaveChangeAsync();

                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.PAYMENT_FAILED;
                    response.Data = "Thanh toán thất bại";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.BusinessCode = BusinessCode.EXCEPTION;
                response.Data = ex.Message;
                return response;
            }
        }
        private bool ValidateSignature(string rspraw, string inputHash, string secretKey)
        {
            string myChecksum = VNPayHelper.HmacSHA512(secretKey, rspraw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}

