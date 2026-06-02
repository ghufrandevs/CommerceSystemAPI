using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CommerceSystemAPI.Models
{
    public class OrderProducts
    {
        public int OrderId { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }

        public int ProductId { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }


        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
