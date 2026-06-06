using System.ComponentModel.DataAnnotations;

namespace CommerceSystemAPI.DTOs
{
    public class PlaceOrderDTO
    {
            

        [Required]
        [MinLength(1)]
        public List<ItemDTO> Items { get; set; }
        }
}
