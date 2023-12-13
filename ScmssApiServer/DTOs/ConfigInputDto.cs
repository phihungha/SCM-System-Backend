using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class ConfigInputDto
    {
        [Required]
        [Range(0, 1)]
        public double VatRate { get; set; }
    }
}
