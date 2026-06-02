using System.ComponentModel.DataAnnotations;

namespace CommerceSystemAPI.DTOs
{
    public class PlaceOrderDTO
    {
            [Required]
            public int UserId { get; set; }

            [Required]
            public List<ItemDTO> Items { get; set; }
        }
}
