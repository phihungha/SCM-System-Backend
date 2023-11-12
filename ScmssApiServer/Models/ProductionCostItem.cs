﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public class ProductionCostItem
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int SupplyId { get; set; }
        public Supply Supply { get; set; } = null!;

        public double Quantity { get; set; }

        [NotMapped]
        public decimal UnitCost => Supply.Price;

        [NotMapped]
        public decimal TotalCost => UnitCost * (decimal)Quantity;
    }
}
