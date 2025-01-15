using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("Recipe")]
    public class Recipe
    {
        [Key] 
        public Guid RecipeID { get; set; }
        public string? RecipeName { get; set; }
        public Guid IngredientID { get; set; }
        [ForeignKey(nameof(RecipeID))]
        public Ingredient? Ingredient { get; set; }

        public Guid UserID { get; set; }
        [ForeignKey(nameof(UserID))]

        public User? User { get; set; }
    }
}
