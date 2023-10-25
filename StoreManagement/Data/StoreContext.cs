using Microsoft.EntityFrameworkCore;
using StoreManagement.Shared.Models;


namespace StoreManagement.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<MarkdownPlan> MarkdownPlans { get; set; }

        public DbSet<SalesDatum> SalesData { get; set; }

        public void SeedData()
        {
            if (!Products.Any())
            {
                var products = new List<Product>
                {
                    new Product { Upc = 100001, Name = "Product A", Description = "Description for Product A", Cost = 10m, Price = 15m },
                    new Product { Upc = 100002, Name = "Product B", Description = "Description for Product B", Cost = 20m, Price = 25m },
                    new Product { Upc = 100003, Name = "Product C", Description = "Description for Product C", Cost = 30m, Price = 35m },
                    new Product { Upc = 100004, Name = "Product D", Description = "Description for Product D", Cost = 40m, Price = 45m },
                    new Product { Upc = 100005, Name = "Product E", Description = "Description for Product E", Cost = 50m, Price = 55m },
                    new Product { Upc = 100006, Name = "Product F", Description = "Description for Product F", Cost = 60m, Price = 65m },
                    new Product { Upc = 100007, Name = "Product G", Description = "Description for Product G", Cost = 70m, Price = 75m },
                    new Product { Upc = 100008, Name = "Product H", Description = "Description for Product H", Cost = 80m, Price = 85m },
                    new Product { Upc = 100009, Name = "Product I", Description = "Description for Product I", Cost = 90m, Price = 95m },
                    new Product { Upc = 100010, Name = "Product J", Description = "Description for Product J", Cost = 100m, Price = 105m }


                };

                Products.AddRange(products);
                SaveChanges();

                var inventories = products.Select(p => new Inventory { ProductId = p.Id, Quantity = new Random().Next(10, 100) }).ToList();

                Inventories.AddRange(inventories);
                SaveChanges();
            }
        }
    }
}
