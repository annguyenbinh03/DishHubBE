using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("Table")]
    public class Table
    {
        public Guid TableID { get; set; }
        public string? TableName { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? QRCode { get; set; }
    }
}
