using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Common.DTO.AdminDTO
{
    public class CreateTableForAdminDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int RestaurantId { get; set; }
    }

}
