using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("RequestType")]
    public class RequestType
    {
        [Key]
        public Guid RequestTypeID { get; set; }
        public string? RequestTypeName { get; set; }
        
    }
}
