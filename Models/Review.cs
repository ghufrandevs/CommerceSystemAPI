using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace CommerceSystemAPI.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime ReviewDate { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
    }
}
