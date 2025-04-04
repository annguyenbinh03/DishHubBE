﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Group6.NET1704.SW392.AIDiner.Common.Model.RegisterLoginModel;

namespace Group6.NET1704.SW392.AIDiner.Common.UserModel
{
    public class UpdateUserModel
    {
        //public int Id { get; set; }

        public string? Username { get; set; }
       
        public string? FullName { get; set; }
        
        public string? Email { get; set; }

        public DateTime? Dob { get; set; }
        
        public string? PhoneNumber { get; set; }
        
        public string? Address { get; set; }
        
        public bool? IsDeleted { get; set; }

        public string? Avatar { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Password { get; set; }


    }
}
