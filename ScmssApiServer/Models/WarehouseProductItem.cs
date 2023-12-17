using AutoMapper;
using ScmssApiServer.DTOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public class WarehouseProductItem : WarehouseItem<WarehouseProductItemEvent>
    {
        [NotMapped]
        public override bool IsActive => Product.IsActive;

        public Product Product { get; set; } = null!;
        public int ProductId { get; set; }

        [NotMapped]
        public override string Unit => Product.Unit;

        [NotMapped]
        public override decimal UnitValue => Product.Price;
    }

    public class WarehouseProductItemMP : Profile
    {
        public WarehouseProductItemMP()
        {
            CreateMap<WarehouseProductItem, WarehouseProductItemDto>();
        }
    }
}
