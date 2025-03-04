using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.BusinessObjects
{
    public class VNPaySettings
    {
        public string VnpayUrl { get; set; }
        public string ReturnUrl { get; set; }
        public string TmnCode { get; set; }
        public string HashSecret { get; set; }
        public string RedirectUrl { get; set; }
    }

}
