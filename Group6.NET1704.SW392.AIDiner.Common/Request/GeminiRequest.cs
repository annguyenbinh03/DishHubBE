using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Common.Request
{
    public class GeminiRequest
    {
        public string Message { get; set; } // Tin nhắn của người dùng (ví dụ: "Tôi muốn đặt một bánh pizza")

        public int orderId { get; set; }
    }
}
