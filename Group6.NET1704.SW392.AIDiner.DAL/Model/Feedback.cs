using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("Feedback")]
    public class Feedback
    {
        [Key]
        public Guid FeedbackID { get; set; }
        public string? UserName { get; set; }
        public DateTime CreateDate { get; set; }
        public string? Comment { get; set; }
        public Guid Rating { get; set; }
        public Guid DishID { get; set; }
        [ForeignKey(nameof(DishID))]
        public Dish? Dish { get; set; }
    }
}
