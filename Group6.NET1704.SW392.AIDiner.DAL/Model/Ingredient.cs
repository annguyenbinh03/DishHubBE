using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("Ingredient")]
    public class Ingredient
    {
        [Key]
        public Guid IngredientID { get; set; }
        public string? IngredientName { get; set; }
        public Guid Quantity { get; set; }
    }
}
