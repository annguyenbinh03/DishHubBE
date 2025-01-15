using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public Guid CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public bool Status { get; set; }
    }
}
