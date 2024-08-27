﻿namespace CS.Core.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int StockAvailable { get; set; }
    }
}
