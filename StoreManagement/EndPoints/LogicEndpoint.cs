using StoreManagement.Data;
using StoreManagement.Shared.Models;

namespace StoreManagement.EndPoints
{
    public static class LogicEndpoint
    {
        public static void MapLogicEndPoints(this WebApplication app)
        {
            app.MapPut("/activateplan/{id}", async (StoreContext context, int id, MarkdownPlan updatedMarkdownPlan) =>
            {

                var markdownPlan = context.MarkdownPlans.Find(id);
                if (markdownPlan == null) return Results.NotFound();

                markdownPlan.IsActive = true;
                markdownPlan.InitialCompleted = true;
                var product = context.Products.Find(markdownPlan.ProductId);
                product.Price = product.Price * (1 - (markdownPlan.InitialReduction * .01m));
                markdownPlan.CurrentSaleDate = markdownPlan.StartDate;
                markdownPlan.InitialInventory = context.Inventories.FirstOrDefault(inv => inv.ProductId == markdownPlan.ProductId).Quantity;

                await context.SaveChangesAsync();
                return Results.Ok();
            });

            app.MapPut("/updatepricesandcounts/{id}", async (StoreContext context, int id, SalesDatum datum) =>
            {
                var salesDatum = context.SalesData.Find(id);
                var markdown = context.MarkdownPlans.Find(salesDatum.MarkdownPlanId);
                var product = context.Products.Find(markdown.ProductId);
                var inventory = context.Inventories.FirstOrDefault(inv => inv.ProductId == product.Id);
                float saleLength = markdown.EndDate.DayNumber - markdown.StartDate.DayNumber;
                int halfwayProgress = markdown.EndDate.DayNumber - datum.SalesDate.DayNumber - 1;
                float saleProgressPercent = 1 - (halfwayProgress / saleLength);

                salesDatum.Margin = product.Price - product.Cost;
                salesDatum.TotalProfit = salesDatum.Margin * salesDatum.TotalSold;
                salesDatum.RemainingInventory = inventory.Quantity - salesDatum.TotalSold;


                if (!markdown.IntermediateCompleted && saleProgressPercent >= .5)
                {
                    product.Price = product.Price * (1 - (markdown.IntermidiateReduction * .01m));
                    markdown.IntermediateCompleted = true;
                    //context.Entry(markdown).State = EntityState.Modified;

                }
                else if (salesDatum.SalesDate == markdown.EndDate.AddDays(-2))
                {
                    product.Price = product.Price * (1 - (markdown.FinalReduction * .01m));
                    //context.Entry(markdown).State = EntityState.Modified;

                }
                else if (salesDatum.SalesDate == markdown.EndDate.AddDays(-1))
                {
                    markdown.SaleEnded = true;
                    markdown.IsActive = false;

                    //context.Entry(markdown).State = EntityState.Modified;
                }


                inventory.Quantity = inventory.Quantity - salesDatum.TotalSold;

                await context.SaveChangesAsync();

                return Results.NoContent();

            });
        }
    }
}
