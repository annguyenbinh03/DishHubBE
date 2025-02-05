using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("DishIngredient")]
    public class DishIngredient
    {
        [Key]
        public Guid DishIngredientID { get; set; }
        public Guid IngredientID { get; set; }
        [ForeignKey(nameof(IngredientID))]
        public Ingredient? Ingredient { get; set; }
        public Guid DishID { get; set; }
        [ForeignKey(nameof(DishID))]

        public Dish? Dish { get; set; }
    }
}
