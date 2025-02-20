using System;
using System.Collections.Generic;

namespace Group6.NET1704.SW392.AIDiner.DAL.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime? Dob { get; set; }
        public string? PhoneNumber { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? Address { get; set; }
        public bool IsDeleted { get; set; }
        public string? Avatar { get; set; }

        public virtual Role? Role { get; set; }
    }
}
