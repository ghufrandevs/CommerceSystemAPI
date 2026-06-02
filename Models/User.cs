using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CommerceSystemAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]

        public string UserEmail { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,}$")]

        public string UserPassword { get; set; }
        [Required]
        public string UserPhone { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; }
        [JsonIgnore]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
