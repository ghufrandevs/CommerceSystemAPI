using System.ComponentModel.DataAnnotations;

namespace CommerceSystemAPI.DTOs
{
    public class LoginDTO
    {
        
            [Required]
            [EmailAddress]
            public string UserEmail { get; set; }

            [Required]
            public string UserPassword { get; set; }
        }
    }

