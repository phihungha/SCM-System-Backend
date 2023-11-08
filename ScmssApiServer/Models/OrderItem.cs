﻿namespace ScmssApiServer.Models
{
    public abstract class OrderItem
    {
        public double Quantity { get; set; }
        public required string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}