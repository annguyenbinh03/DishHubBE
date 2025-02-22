using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Common.DTO.Request
{
    public class UpdateRequestDTO
    {
        [Required]
        public string Status { get; set; }
    }
}
