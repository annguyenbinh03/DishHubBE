using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("Menu")]
    public class Menu
    {
        public Guid MenuID { get; set; }
        public string? MenuName { get; set; }
        public string? Description { get; set; }
    }
}
