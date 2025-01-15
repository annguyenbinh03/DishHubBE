using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("ChatbotAI")]
    public class ChatbotAI
    {
        [Key]
        public Guid ChatbotID { get; set; }
        public string? Version { get; set; }
        public bool Status { get; set; }
        public Guid UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public User? User { get; set; }
        public Guid DishID { get; set; }
        [ForeignKey(nameof(DishID))]

        public Dish? Dish { get; set; }
    }
}
