using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.PaymentGateWay
{
    public class VnpayService : IVnpayService
    {
        private IConfiguration _configuration;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Payment> _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VnpayService(IConfiguration configuration, IGenericRepository<Order> orderRepository, IGenericRepository<Payment> paymentRepository, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
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
                decimal totalAmount = order.TotalAmount;
                if (totalAmount <= 0)
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.INVALID_INPUT;
                    response.Data = "Total amount must be greater than 0";
                    return response;
                }
                string transactionCode = Guid.NewGuid().ToString();
                var newPayment = new Payment
                {
                    OrderId = orderId,
                    MethodId = methodId,
                    TransactionCode = transactionCode,
                    CreatedAt = DateTime.UtcNow,
                    Amount = totalAmount,
                    Status = false
                };
                await _paymentRepository.Insert(newPayment);
                await _unitOfWork.SaveChangeAsync();
                string paymentUrl = CreatePaymentUrl(totalAmount, orderId);

                response.IsSucess = true;
                response.BusinessCode = BusinessCode.CREATE_SUCCESS;
                response.Data = new { PaymentUrl = paymentUrl, TransactionCode = transactionCode };

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
            string tmnCode = _configuration["Vnpay:TmnCode"];
            string hashSecret = _configuration["Vnpay:HashSecret"];
            string returnUrl = _configuration["Vnpay:ReturnUrl"];
            string vnpayUrl = _configuration["Vnpay:VnpayUrl"];

            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", tmnCode);
            vnpay.AddRequestData("vnp_Amount", ((int)(amount * 100)).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", "127.0.0.1");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toán đơn hàng: {orderId}");
            vnpay.AddRequestData("vnp_OrderType", "billpayment");
            vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);
            vnpay.AddRequestData("vnp_TxnRef", orderId.ToString());

            return vnpay.CreateRequestUrl(vnpayUrl, hashSecret);
        }

        public async Task<ResponseDTO> ConfirmPayment(Dictionary<string, string> queryParams)
        {
            ResponseDTO response = new ResponseDTO();
            try
            {
                // 1. Kiểm tra các tham số quan trọng từ VNPAY
                if (!queryParams.TryGetValue("vnp_ResponseCode", out string vnp_ResponseCode) ||
                    !queryParams.TryGetValue("vnp_TxnRef", out string vnp_TxnRef) ||
                    !queryParams.TryGetValue("vnp_TransactionNo", out string vnp_TransactionNo))
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.INVALID_INPUT;
                    response.Data = "Thiếu thông tin từ VNPAY";
                    return response;
                }

                // 2. Parse orderId từ vnp_TxnRef
                if (!int.TryParse(vnp_TxnRef, out int orderId))
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.INVALID_INPUT;
                    response.Data = "Không lấy được orderId từ vnp_TxnRef";
                    return response;
                }

                var order = await _orderRepository.GetById(orderId);
                if (order == null)
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.NOT_FOUND;
                    response.Data = "Không tìm thấy đơn hàng";
                    return response;
                }

                // 3. Kiểm tra chữ ký VNPAY (nếu có)
                if (!queryParams.TryGetValue("vnp_SecureHash", out string vnp_SecureHash) ||
                    !ValidateSignature(queryParams, vnp_SecureHash))
                {
                    response.IsSucess = false;
                    response.Data = "Chữ ký VNPAY không hợp lệ";
                    return response;
                }

                // 4. Xử lý thanh toán nếu thành công (vnp_ResponseCode == "00")
                if (vnp_ResponseCode == "00")
                {
                    var payment = new Payment
                    {
                        OrderId = orderId,
                        MethodId = 1, // VNPAY
                        TransactionCode = vnp_TransactionNo,
                        CreatedAt = DateTime.UtcNow,
                        Amount = order.TotalAmount,
                        Description = $"Thanh toán thành công cho orderId {orderId}",
                        Status = true,
                    };

                    await _paymentRepository.Insert(payment);
                    order.Status = "completed";
                    await _orderRepository.Update(order);
                    await _unitOfWork.SaveChangeAsync();

                    response.IsSucess = true;
                    response.BusinessCode = BusinessCode.CREATE_SUCCESS;
                    response.Data = new { PaymentId = payment.Id, RedirectUrl = $"https://dishhub-dxfrckc2c3fjgch4.southeastasia-01.azurewebsites.net/{payment.Id}" };
                    return response;
                }
                else
                {
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
        private bool ValidateSignature(Dictionary<string, string> queryParams, string inputHash)
        {
            if (!queryParams.ContainsKey("vnp_SecureHash"))
                return false;

            string secretKey = _configuration["Vnpay:HashSecret"];
            Dictionary<string, string> sortedParams = queryParams
        .Where(x => x.Key != "vnp_SecureHash" && x.Key != "vnp_SecureHashType")
        .OrderBy(x => x.Key)
        .ToDictionary(x => x.Key, x => x.Value);


            string rawData = string.Join("&", sortedParams.Select(x => $"{x.Key}={x.Value}"));
            string myChecksum = Utils.HmacSHA512(secretKey, rawData);

            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }


    }
    }

