using Group6.NET1704.SW392.AIDiner.Common.Request;
using Group6.NET1704.SW392.AIDiner.Common.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Group6.NET1704.SW392.AIDiner.Services.Implementation
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/tunedModels/testmodel-9yz6b1wcf4i6:generateContent"; // Thay đổi theo mô hình và endpoint bạn sử dụng

        public GeminiService(string apiKey) // Sử dụng DI nếu có thể.
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }

        public async Task<GeminiResponse> ProcessUserMessage(string message)
        {
            string examples = @"
    Ví dụ 1:
    Tin nhắn: Tôi muốn đặt một Pizza Margherita
    JSON: { ""Intent"": ""OrderFood"", ""FoodId"": ""101"", ""Quantity"": ""1"", ""ResponseText"": ""OK, tôi đã đặt một Pizza Margherita cho bạn."" }

    Ví dụ 2:
    Tin nhắn: Cho tôi xem menu đi
    JSON: { ""Intent"": ""GetMenu"",  ""ResponseText"": ""Đây là menu của chúng tôi..."" }

    Ví dụ 3:
    Tin nhắn: Nhà hàng mở cửa lúc mấy giờ?
    JSON: { ""Intent"": ""GetRestaurantInfo"", ""ResponseText"": ""Nhà hàng mở cửa từ 10:00 sáng đến 10:00 tối hàng ngày."" }

    Ví dụ 4:
    Tin nhắn: Pizza Margherita có những thành phần gì?
    JSON: { ""Intent"": ""GetDishInformation"", ""ResponseText"": ""Pizza Margherita bao gồm cà chua, mozzarella và базилик."" }
    ";



            string prompt = $"Bạn là một trợ lý ảo cho một nhà hàng. Hãy phân tích tin nhắn của người dùng và trả về một JSON object với thông tin sau:\n" +
                      "* `Intent`: Ý định của người dùng (ví dụ: OrderFood, GetMenu, GetRestaurantInfo, GetDishInformation).\n" +
                      "* Các thông tin liên quan (nếu có), ví dụ: `FoodId`, `Quantity`.\n" +
                      "* `ResponseText`:  Phản hồi đầy đủ cho người dùng, sử dụng kiến thức bạn đã học được.\n" +
                      "\n" +
                      "Dưới đây là một vài ví dụ:\n" +
                      examples +
                      "\n" +
                      $"Tin nhắn của người dùng: {message}\n" +
                      "JSON:";

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
                    text = prompt
                }
            }
        }
    },
                generationConfig = new
                {
                    temperature = 0.1
                }
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            string url = $"{BaseUrl}?key={_apiKey}";

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);


            Console.WriteLine($"Request URL: {url}");  // Ghi URL yêu cầu

            // Kiểm tra trạng thái và nội dung của phản hồi
            Console.WriteLine($"Response Status Code: {response.StatusCode}");
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {responseContent}");


            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Full Gemini Response: {jsonResponse}");

                dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);

                if (responseObject?.candidates == null || responseObject.candidates.Count == 0 ||
                    responseObject.candidates[0]?.content?.parts == null || responseObject.candidates[0].content.parts.Count == 0)
                {
                    Console.WriteLine("Unexpected response structure from Gemini.");
                    return new GeminiResponse { Intent = "Unknown", ResponseText = "Tôi xin lỗi, có lỗi xảy ra khi xử lý phản hồi." };
                }

                string geminiOutput = responseObject.candidates[0].content.parts[0].text.ToString().Trim();
                geminiOutput = geminiOutput.Replace("`", "").Trim();

                // Loại bỏ "json" và bất kỳ khoảng trắng nào ở đầu chuỗi
                if (geminiOutput.StartsWith("json", StringComparison.OrdinalIgnoreCase))
                {
                    geminiOutput = geminiOutput.Substring(4).Trim(); // Loại bỏ "json" (4 ký tự) và khoảng trắng
                }


                Console.WriteLine($"Gemini Output before parsing: {geminiOutput}");

                try
                {
                    JToken.Parse(geminiOutput);

                    GeminiResponse geminiResponse = JsonConvert.DeserializeObject<GeminiResponse>(geminiOutput);
                    return geminiResponse;
                }
                catch (JsonReaderException ex)
                {
                    Console.WriteLine($"Invalid JSON: {ex.Message}");
                    Console.WriteLine($"Gemini Output: {geminiOutput}");
                    return new GeminiResponse { Intent = "Unknown", ResponseText = "Tôi xin lỗi, tôi không hiểu câu hỏi của bạn." };
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return new GeminiResponse { Intent = "Unknown", ResponseText = "Có lỗi xảy ra khi xử lý yêu cầu của bạn." };
            }
        }
    }
}
