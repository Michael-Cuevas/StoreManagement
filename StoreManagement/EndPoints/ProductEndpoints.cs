using StoreManagement.Data;
using StoreManagement.Shared.Models;

namespace StoreManagement.EndPoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            //################# Products #############

            // READ all products
            app.MapGet("/products", (StoreContext context) =>
            {
                return context.Products.ToList();
            });

            // CREATE a new product
            app.MapPost("/products", (StoreContext context, Product product) =>
            {
                context.Products.Add(product);
                context.SaveChanges();
                return Results.Created($"/products/{product.Id}", product);
            });

            // READ product by ID
            app.MapGet("/products/{id}", async (StoreContext context, int id) =>
                await context.Products.FindAsync(id)
                is Product product
                    ? Results.Ok(product)
                    : Results.NotFound());

            // UPDATE product by ID
            app.MapPut("/products/{id}", async (StoreContext context, int id, Product updatedProduct) =>
            {
                var product = context.Products.Find(id);
                if (product == null) return Results.NotFound();

                product.Name = updatedProduct.Name;
                product.Description = updatedProduct.Description;

                context.SaveChanges();

                await context.SaveChangesAsync();

                return Results.NoContent();

            });

            // DELETE product by ID
            app.MapDelete("/products/{id}", (StoreContext context, int id) =>
            {
                var product = context.Products.Find(id);
                if (product == null) return Results.NotFound();

                context.Products.Remove(product);
                context.SaveChanges();
                return Results.Ok();
            });

        }
    }
}