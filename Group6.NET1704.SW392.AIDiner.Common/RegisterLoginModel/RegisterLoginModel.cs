using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Group6.NET1704.SW392.AIDiner.DAL.ViewModel
{
    public class RegisterLoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(5, ErrorMessage = "User name must be at least 5 characters long.")]
        [RegularExpression(@"^[a-zA-Z0-9\sÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẮẰẲẴẶắằẳẵặƯứừửữự]+$",
            ErrorMessage = "UserName must not contain special characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [MinLength(10, ErrorMessage = "Full name must be at least 10 characters long.")]
        [MaxLength(200, ErrorMessage = "Full name must be at most 200 characters long.")]
        [RegularExpression(@"^[^!@#$%^&*()_+=\[{\]};:<>|./?,-]*$",
            ErrorMessage = "Full name must not contain special characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
        [RegularExpression(@"^[\w-\.]+@(gmail\.com)$",
            ErrorMessage = "Email must be a valid Gmail address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password must be at least 8 characters long", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+=\[{\]};:<>|./?,-])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(RegisterLoginModel), nameof(ValidateDob))]
        public DateTime? Dob { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone number must start with 0 and be 10 digits long.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(100, ErrorMessage = "Address must be at most 100 characters long.")]
        [RegularExpression(@"^[^!@#$%^&*()_+=\[{\]};:<>|?-]*$",
            ErrorMessage = "Address must not contain special characters.")]
        public string Address { get; set; }
        //[Required(ErrorMessage = "Avatar is required.")]
        //[Url(ErrorMessage = "Avatar must be a valid URL.")]
        public string Avatar { get; set; }

        public static ValidationResult ValidateDob(DateTime? dob, ValidationContext context)
        {
            if (dob.HasValue && dob.Value >= DateTime.Today)
            {
                return new ValidationResult("Date of birth must be in the past.");
            }
            return ValidationResult.Success;
        }
    }
}
