using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Common.Request
{
    public class ChangeOrderDetailStatusRequest
    {
        [Required]
        public string status { set; get; }
    }
}
