using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("PaymentMethod")]
    public class PaymentMethod
    {
        [Key] 
        public Guid PaymentMethodID { get; set; }
        public string? PaymentMethodName { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }

    }
}
