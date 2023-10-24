using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.Shared.Models;

namespace StoreManagement.EndPoints
{
    public static class InventoryEndpoints
    {
        public static void MapInventoryEndpoints(this WebApplication app)
        {
            //################# Inventory #############

            // CREATE new product
            app.MapPost("/inventory", (StoreContext context, Inventory inventory) =>
            {
                context.Inventories.Add(inventory);
                context.SaveChanges();
                return Results.Created($"/products/{inventory.Id}", inventory);
            });

            // READ all products
            app.MapGet("/inventory", (StoreContext context) =>
            {
                return context.Inventories.Include(i => i.Product).ToList();
            });

            // READ product by ID
            app.MapGet("/inventory/{id}", async (StoreContext context, int id) =>
                await context.Inventories.FindAsync(id)
                is Inventory inventoryItem
                    ? Results.Ok(inventoryItem)
                    : Results.NotFound());

            // UPDATE product by ID
            app.MapPut("/inventory/{id}", async (StoreContext context, int id, Product updatedProduct) =>
            {
                var inventoryItem = context.Products.Find(id);
                if (inventoryItem == null) return Results.NotFound();

                inventoryItem.Name = updatedProduct.Name;
                inventoryItem.Description = updatedProduct.Description;

                context.SaveChanges();

                await context.SaveChangesAsync();

                return Results.NoContent();

            });

            // DELETE product by ID
            app.MapDelete("/inventory/{id}", (StoreContext context, int id) =>
            {
                var inventoryItem = context.Products.Find(id);
                if (inventoryItem == null) return Results.NotFound();

                context.Products.Remove(inventoryItem);
                context.SaveChanges();
                return Results.Ok();
            });
        }
    }
}
