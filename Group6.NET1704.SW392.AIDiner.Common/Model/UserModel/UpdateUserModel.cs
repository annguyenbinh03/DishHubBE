using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Group6.NET1704.SW392.AIDiner.Common.Model.RegisterLoginModel;

namespace Group6.NET1704.SW392.AIDiner.Common.UserModel
{
    public class UpdateUserModel
    {
        public int Id { get; set; }
        [MinLength(5, ErrorMessage = "User name must be at least 5 characters long.")]
        [RegularExpression(@"^[a-zA-Z0-9\sÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẮẰẲẴẶắằẳẵặƯứừửữự]+$",
            ErrorMessage = "UserName must not contain special characters.")]
        public string? Username { get; set; }
        [MinLength(10, ErrorMessage = "Full name must be at least 10 characters long.")]
        [MaxLength(200, ErrorMessage = "Full name must be at most 200 characters long.")]
        [RegularExpression(@"^[^!@#$%^&*()_+=\[{\]};:<>|./?,-]*$",
            ErrorMessage = "Full name must not contain special characters.")]
        public string? FullName { get; set; }
        [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
        [RegularExpression(@"^[\w-\.]+@(gmail\.com)$",
            ErrorMessage = "Email must be a valid Gmail address.")]
        public string? Email { get; set; }
        [DataType(DataType.Date)]
        [CustomValidation(typeof(RegisterLoginModel), nameof(ValidateDob))]
        public DateTime? Dob { get; set; }
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone number must start with 0 and be 10 digits long.")]
        public string? PhoneNumber { get; set; }
        [MaxLength(100, ErrorMessage = "Address must be at most 100 characters long.")]
        [RegularExpression(@"^[^!@#$%^&*()_+=\[{\]};:<>|?-]*$",
            ErrorMessage = "Address must not contain special characters.")]
        public string? Address { get; set; }
        [CustomValidation(typeof(UpdateUserModel), nameof(ValidateStatus))]
        public bool? Status { get; set; }
        public string? Avatar { get; set; }

        public static ValidationResult ValidateDob(DateTime? dob, ValidationContext context)
        {
            if (dob.HasValue && dob.Value >= DateTime.Today)
            {
                return new ValidationResult("Date of birth must be in the past.");
            }
            return ValidationResult.Success;
        }

        public static ValidationResult? ValidateStatus(bool? status, ValidationContext context)
        {
            if (status.HasValue && status != true && status != false)
            {
                return new ValidationResult("Status must be either true or false.");
            }
            return ValidationResult.Success;
        }
    }
}
