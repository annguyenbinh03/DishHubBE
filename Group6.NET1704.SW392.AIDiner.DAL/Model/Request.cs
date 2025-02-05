using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("Request")]
    public class Request
    {
        [Key]
        public Guid RequestID { get; set; }
        public string? Note { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime ProcessAt { get; set; }
        public Guid OrderID { get; set; }
        [ForeignKey(nameof(OrderID))]
        public Order? Order { get; set; }

        public Guid RequestTypeID { get; set; }
        [ForeignKey(nameof(RequestTypeID))]
        public RequestType? RequestType { get; set; }
    }
}
