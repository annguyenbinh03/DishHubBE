using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("UserRole")]

    public class UserRole
    {
        [Key] 
        public Guid RoleID { get; set; }
        public string? RoleName { get; set; }
    }
}
