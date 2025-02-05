using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("Dish")]
    public class Dish
    {
        [Key]
        public Guid DishID { get; set; }
        public string? DishName { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }

        public Guid MenuID { get; set; }
        [ForeignKey(nameof(MenuID))]

        public Table? Menu { get; set; }

        public Guid CategoryID { get; set; }
        [ForeignKey(nameof(CategoryID))]    
        public Category? Category { get; set; }
    }
}
