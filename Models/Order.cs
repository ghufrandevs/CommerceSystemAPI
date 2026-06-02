using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace CommerceSystemAPI.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderProducts>? OrderProduct { get; set; }
    }
}
