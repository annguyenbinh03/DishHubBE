using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Common.Response
{
    public class GeminiResponse
    {
        public string Intent { get; set; } // Ý định của người dùng (ví dụ: "OrderFood", "GetMenu", "CheckOrderStatus")
        public Dictionary<string, string> Entities { get; set; } // Các thực thể (ví dụ: {"food": "pizza", "quantity": "1"})
        public string ResponseText { get; set; } // Thêm thuộc tính này
        public string FoodId { get; set; } // Thêm thuộc tính này
        public string Quantity { get; set; } // Thêm thuộc tính này
    }
}
