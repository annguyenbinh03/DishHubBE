using System;
using System.Collections.Generic;

namespace Group6.NET1704.SW392.AIDiner.DAL.Models
{
    public partial class WishList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DishId { get; set; }

        public virtual Dish Dish { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
