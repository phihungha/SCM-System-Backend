﻿using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class PurchaseOrderProgressUpdate : OrderProgressUpdate
    {
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
    }

    public class PurchaseOrderProgressUpdateMappingProfile : Profile
    {
        public PurchaseOrderProgressUpdateMappingProfile()
        {
            CreateMap<OrderProgressUpdateInputDto, PurchaseOrderProgressUpdate>();
        }
    }
}
