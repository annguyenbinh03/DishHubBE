using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Common.DTO.Request
{
    public class UpdateTableRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? RestaurantId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
