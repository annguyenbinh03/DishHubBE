using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL
{
    [Table("User")]

    public class User
    {
        public Guid UserID { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid RoleID { get; set; }
        [ForeignKey(nameof(RoleID))]    

        public UserRole? UserRole { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Address { get; set; }
        public bool Status { get; set; }


    }
}
