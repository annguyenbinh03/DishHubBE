using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("FavoriteRecipe")]
    public class FavoriteRecipe
    {
        [Key]
        public Guid FavoriteRecipeID { get; set; }
        public Guid DishID { get; set; }
        [ForeignKey(nameof(FavoriteRecipeID))]

        public Dish? Dish { get; set; }
    }
}
