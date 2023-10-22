﻿namespace StoreManagement.Models
{
    public class Product
    {
        public int Id { get; set; }

        public int Upc { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Cost { get; set; }

        public decimal Price { get; set; }
    }
}
