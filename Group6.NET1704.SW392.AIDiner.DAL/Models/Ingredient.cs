using System;
using System.Collections.Generic;

namespace Group6.NET1704.SW392.AIDiner.DAL.Models
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            DishIngredients = new HashSet<DishIngredient>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<DishIngredient> DishIngredients { get; set; }
    }
}
