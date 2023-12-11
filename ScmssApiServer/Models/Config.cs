using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    /// <summary>
    /// Contains system configuration.
    /// </summary>
    public class Config
    {
        public int Id { get; set; }

        /// <summary>
        /// VAT tax rate from 0 to 1 (0% to 100%).
        /// </summary>
        public double VatRate { get; set; }
    }

    public class ConfigMp : Profile
    {
        public ConfigMp()
        {
            CreateMap<ConfigInputDto, Config>();
        }
    }
}
