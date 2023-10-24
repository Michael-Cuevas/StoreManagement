using StoreManagement.Data;
using StoreManagement.Shared.Models;

namespace StoreManagement.EndPoints
{
    public static class SalesDatumEndpoints
    {
        public static void MapSalesDatumEndPoints(this WebApplication app)
        {
            //################# SalesDatum #############

            // CREATE a new sales datum
            app.MapPost("/salesdata", (StoreContext context, SalesDatum salesDatum) =>
            {
                context.SalesData.Add(salesDatum);
                context.SaveChanges();

                return Results.Created($"/salesdata/{salesDatum.Id}", salesDatum);
            });

            // READ all sales data for a markdown plan
            app.MapGet("/salesdata/forplan/{planId}", (StoreContext context, int planId) =>
            {
                return context.SalesData.Where(sd => sd.MarkdownPlanId == planId).ToList() ?? null;
            });

            // READ a single sales datum by its ID
            app.MapGet("/salesdata/{id}", async (StoreContext context, int id) =>
                await context.SalesData.FindAsync(id)
                is SalesDatum salesDatum
                    ? Results.Ok(salesDatum)
                    : Results.NotFound());

            // UPDATE a sales datum by its ID
            app.MapPut("/salesdata/{id}", async (StoreContext context, int id, SalesDatum updatedSalesDatum) =>
            {
                var salesDatum = context.SalesData.Find(id);
                if (salesDatum == null) return Results.NotFound();

                salesDatum.SalesDate = updatedSalesDatum.SalesDate;
                salesDatum.TotalSold = updatedSalesDatum.TotalSold;
                salesDatum.TotalProfit = updatedSalesDatum.TotalProfit;
                salesDatum.MarkdownPlanId = updatedSalesDatum.MarkdownPlanId;
                salesDatum.MarkdownPlan = updatedSalesDatum.MarkdownPlan;
                salesDatum.RemainingInventory = updatedSalesDatum.RemainingInventory;
                salesDatum.Margin = updatedSalesDatum.Margin;


                context.SaveChanges();

                await context.SaveChangesAsync();

                return Results.NoContent();

            });

            // DELETE a sales datum by its ID
            app.MapDelete("/salesdata/{id}", (StoreContext context, int id) =>
            {
                var salesDatum = context.SalesData.Find(id);
                if (salesDatum == null) return Results.NotFound();

                context.SalesData.Remove(salesDatum);
                context.SaveChanges();
                return Results.Ok();
            });
        }
    }
}
