using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Common.DTO
{
    public class ResponseDTO
    {
        public bool IsSucess { get; set; } = true;
        public object Data { get; set; }
        public BusinessCode.BusinessCode BusinessCode { get; set; }
    }
}
