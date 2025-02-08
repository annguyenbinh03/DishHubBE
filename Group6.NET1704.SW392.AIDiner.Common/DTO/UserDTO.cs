using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Common.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime? Dob { get; set; }
        public string PhoneNumber { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreateAt { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
        public string Avatar { get; set; }
    }


}
