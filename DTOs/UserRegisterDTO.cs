using System.ComponentModel.DataAnnotations;

namespace CommerceSystemAPI.DTOs
{
    public class UserRegisterDTO
    {
        
            [Required]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            public string UserEmail { get; set; }

            [Required]
            [RegularExpression(
                @"^(?=.*[A-Z])(?=.*\d).{8,}$",
                ErrorMessage = "Password must contain at least one uppercase letter and one number.")]
            public string UserPassword { get; set; }

            [Required]
            public string UserPhone { get; set; }
        }
}
