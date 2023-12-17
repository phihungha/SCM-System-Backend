using AutoMapper;
using ScmssApiServer.DTOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public class WarehouseSupplyItem : WarehouseItem<WarehouseSupplyItemEvent>
    {
        public Supply Supply { get; set; } = null!;
        public int SupplyId { get; set; }

        [NotMapped]
        public override string Unit => Supply.Unit;

        [NotMapped]
        public override decimal UnitValue => Supply.Price;
    }

    public class WarehouseSupplyItemMP : Profile
    {
        public WarehouseSupplyItemMP()
        {
            CreateMap<WarehouseSupplyItem, WarehouseSupplyItemDto>();
        }
    }
}
