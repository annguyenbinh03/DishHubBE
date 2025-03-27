using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Common.DTO.AdminDTO
{
    public class TableAdminDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string Status { get; set; } = null!;
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = null!;
        public string? RestaurantImage { get; set; }

    }
}
