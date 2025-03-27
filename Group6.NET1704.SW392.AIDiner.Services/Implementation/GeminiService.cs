using Group6.NET1704.SW392.AIDiner.Common.Request;
using Group6.NET1704.SW392.AIDiner.Common.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Microsoft.Extensions.Configuration;

namespace Group6.NET1704.SW392.AIDiner.Services.Implementation
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/tunedModels/dishhubai-43kbrp9gl2k9:generateContent"; // Thay đổi theo mô hình và endpoint bạn sử dụng
        private readonly IOrderDetailService _orderDetailService;
        private readonly IUnitOfWork _unitOfWork;

        public GeminiService(IConfiguration configuration, IOrderDetailService? orderDetailService, IUnitOfWork? unitOfWork) // Sử dụng DI nếu có thể.
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["Gemini:Key"];
            _orderDetailService = orderDetailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> OrderFood(GeminiResponse processedRequest)
        {
            string response = "";
            if (string.IsNullOrEmpty(processedRequest.FoodId) || !int.TryParse(processedRequest.FoodId, out int foodId))
            {
                response = "Error: FoodId is required for OrderFood intent.";
                return response;
            }

            if (!int.TryParse(processedRequest.Quantity, out int quantity))
            {
                response = "Error: Invalid food quantity";
                return response;
            }
            List<DishRequestDTO> listDish = new();
            listDish.Add(
                new DishRequestDTO
                {
                    DishId = foodId,
                    Quantity = quantity
                }
            );
            ResponseDTO responseDTO = await _orderDetailService.AddDishToOrder(processedRequest.OrderId, listDish);
            if(responseDTO.IsSucess)
            {
                Dish dish = await _unitOfWork.Dishes.GetById(foodId);
                response = "Tôi đã đặt " + quantity.ToString() + " phần " + dish.Name.ToString() + " cho bạn. Bạn có cần trợ giúp gì thêm không?";
                return response;
            }
            else
            {
                response = "Order food fail: " + responseDTO.message;
                return response;
            }
        }

        public async Task<GeminiResponse> ProcessUserMessage(string message, int orderId)
        {
            // Logging các tham số đầu vào
            Console.WriteLine($"Processing message: '{message}' for orderId: {orderId}");

            try
            {
                // Xây dựng payload JSON
                var payload = new
                {
                    contents = new[]
                    {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = message
                            }
                        }
                    }
                },
                    generationConfig = new
                    {
                        temperature = 0 // Đặt temperature mặc định
                    }
                };

                string jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Ghi log payload trước khi gửi yêu cầu
                Console.WriteLine($"Sending payload: {jsonPayload}");

                // Tạo URL yêu cầu
                string url = $"{BaseUrl}?key={_apiKey}"; // Use _baseUrl here
                Console.WriteLine($"Request URL: {url}"); // Ghi URL yêu cầu

                // Gửi yêu cầu và lấy phản hồi
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                // Đọc nội dung phản hồi
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Full Gemini Response: {responseContent}"); // Ghi toàn bộ phản hồi

                // Xử lý phản hồi thành công
                if (response.IsSuccessStatusCode)
                {
                    // Ghi log trạng thái thành công
                    Console.WriteLine("Request succeeded.");

                    // Kiểm tra nội dung rỗng
                    if (string.IsNullOrEmpty(responseContent))
                    {
                        Console.WriteLine("Response content is empty.");
                        return new GeminiResponse { Intent = "Unknown", ResponseText = "Tôi xin lỗi, phản hồi từ máy chủ bị rỗng." };
                    }

                    try
                    {
                        dynamic responseObject = JsonConvert.DeserializeObject(responseContent); // Deserialize toàn bộ phản hồi

                        // Trích xuất văn bản và JSON một cách riêng biệt. Giả định định dạng là "text { JSON }" hoặc "{ JSON }".
                        string geminiOutput = responseObject?.candidates?[0]?.content?.parts?[0]?.text?.ToString()?.Trim() ?? "";
                        geminiOutput = geminiOutput.Replace("`", "").Trim(); // Loại bỏ backticks

                        string responseText = "";
                        string jsonString = "";

                        // Cố gắng tách phản hồi thành các phần văn bản và JSON
                        if (geminiOutput.StartsWith("{") && geminiOutput.EndsWith("}")) // Phản hồi JSON trực tiếp
                        {
                            jsonString = geminiOutput;
                        }
                        else
                        {
                            int jsonStartIndex = geminiOutput.LastIndexOf('{');
                            if (jsonStartIndex != -1)
                            {
                                responseText = geminiOutput.Substring(0, jsonStartIndex).Trim();
                                jsonString = geminiOutput.Substring(jsonStartIndex).Trim(); // Trích xuất phần JSON
                            }
                            else
                            {
                                // Không tìm thấy JSON; coi toàn bộ đầu ra là văn bản phản hồi
                                responseText = geminiOutput.Trim();
                                return new GeminiResponse { Intent = "Unknown", ResponseText = responseText };
                            }
                        }

                        Console.WriteLine($"Response Text: {responseText}");
                        Console.WriteLine($"JSON String: {jsonString}");

                        // Phân tích cú pháp JSON (nếu có)
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            try
                            {
                                // Làm sạch JSON. Giả định rằng nó có thể không có dấu ngoặc kép xung quanh tên thuộc tính.
                                string fixedJsonString = Regex.Replace(jsonString, @"(\w+):", @"""$1"":");

                                // Deserialize Json
                                GeminiResponse geminiResponse = JsonConvert.DeserializeObject<GeminiResponse>(fixedJsonString);
                                geminiResponse.ResponseText = responseText; // Gán ResponseText
                                geminiResponse.OrderId = orderId;
                                return geminiResponse;
                            }
                            catch (JsonReaderException ex)
                            {
                                Console.WriteLine($"Invalid JSON: {ex.Message}");
                                Console.WriteLine($"JSON being parsed: {jsonString}");
                                return new GeminiResponse { Intent = "Unknown", ResponseText = responseText }; // Gán ResponseText nếu có lỗi
                            }
                        }
                        else
                        {
                            // Nếu chỉ có văn bản
                            return new GeminiResponse { Intent = "Unknown", ResponseText = responseText };
                        }

                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Error deserializing response content: {ex.Message}");
                        Console.WriteLine($"Response Content: {responseContent}"); // In response content
                        return new GeminiResponse { Intent = "Unknown", ResponseText = "Có lỗi xảy ra khi xử lý phản hồi." };
                    }
                }
                else
                {
                    // Xử lý phản hồi lỗi
                    Console.WriteLine($"Error: {response.StatusCode} - {responseContent}"); // In chi tiết lỗi
                    return new GeminiResponse { Intent = "Unknown", ResponseText = "Có lỗi xảy ra khi xử lý yêu cầu của bạn." };
                }
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi liên quan đến HttpClient
                Console.WriteLine($"HTTP request error: {ex.Message}");
                return new GeminiResponse { Intent = "Unknown", ResponseText = "Không thể kết nối đến máy chủ." };
            }
            catch (TaskCanceledException ex)
            {
                // Xử lý lỗi timeout
                Console.WriteLine($"Request timed out: {ex.Message}");
                return new GeminiResponse { Intent = "Unknown", ResponseText = "Yêu cầu vượt quá thời gian quy định." };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi không mong muốn
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return new GeminiResponse { Intent = "Unknown", ResponseText = "Có lỗi nghiêm trọng xảy ra. Vui lòng thử lại sau." };
            }
        }

    }
}
