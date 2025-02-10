using System;
using System.Collections.Generic;

namespace Group6.NET1704.SW392.AIDiner.DAL.Models
{
    public partial class DishIngredient
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public int IngredientId { get; set; }

        public virtual Dish Dish { get; set; } = null!;
        public virtual Ingredient Ingredient { get; set; } = null!;
    }
}
