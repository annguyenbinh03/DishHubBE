using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Common.Response
{
    public class RestaurantUpdateReponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
