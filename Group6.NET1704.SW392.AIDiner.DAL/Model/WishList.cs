using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("WishList")]
    public class WishList
    {
        [Key]
        public Guid WishListID { get; set; }
        public Guid DishID { get; set; }
        [ForeignKey(nameof(DishID))]

        public Dish? Dish { get; set; }
        public Guid UserID { get; set; }
        [ForeignKey(nameof(UserID))]

        public User? User { get; set; }

    }
}
