using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("Maintenance")]
    public class Maintenance
    {
        [Key] 
        public Guid MaintenanceID { get; set; }
        public DateTime Date { get; set; }
        public string? Technician { get; set; }
        public string? Description { get; set; }

        public Guid ChatbotID { get; set; }
        [ForeignKey(nameof(ChatbotID))]
        public ChatbotAI? ChatbotAI { get; set; }
    }
}
