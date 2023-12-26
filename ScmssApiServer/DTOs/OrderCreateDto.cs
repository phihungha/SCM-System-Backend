using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class OrderCreateDto<TItemDto>
    {
        [Required]
        public required ICollection<TItemDto> Items { get; set; }
    }
}
