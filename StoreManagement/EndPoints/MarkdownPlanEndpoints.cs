using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.Shared.Models;

namespace StoreManagement.EndPoints
{
    public static class MarkdownPlanEndpoints
    {
        public static void MapMarkdownPlanEndpoints(this WebApplication app)
        {
            //################# MarkdownPlan #############

            // CREATE markdown plan
            app.MapPost("/markdownplan", (StoreContext context, MarkdownPlan markdownPlan) =>
            {
                context.MarkdownPlans.Add(markdownPlan);
                context.SaveChanges();

                //Load related product into markdownplan
                context.Entry(markdownPlan).Reference(m => m.Product).Load();

                return Results.Created($"/markdownplan/{markdownPlan.Id}", markdownPlan);
            });
            // READ all markdown plans
            app.MapGet("/markdownplan", (StoreContext context) =>
            {
                return context.MarkdownPlans.Include(m => m.Product).ToList();
            });

            // READ markdown plan by ID
            app.MapGet("/markdownplan/{id}", async (StoreContext context, int id) =>
            {
                var markdownItem = await context.MarkdownPlans.Include(m => m.Product).FirstOrDefaultAsync(m => m.Id == id);
                return markdownItem != null ? Results.Ok(markdownItem) : Results.NotFound();
            });


            // UPDATE markdown plan by ID
            app.MapPut("/markdownplan/{id}", async (StoreContext context, int id, MarkdownPlan updatedMarkdownPlan) =>
            {
                var markdownPlan = context.MarkdownPlans.Find(id);
                if (markdownPlan == null) return Results.NotFound();

                markdownPlan.StartDate = updatedMarkdownPlan.StartDate;
                markdownPlan.EndDate = updatedMarkdownPlan.EndDate;
                markdownPlan.InitialReduction = updatedMarkdownPlan.InitialReduction;
                markdownPlan.IntermidiateReduction = updatedMarkdownPlan.IntermidiateReduction;
                markdownPlan.FinalReduction = updatedMarkdownPlan.FinalReduction;
                markdownPlan.IsActive = updatedMarkdownPlan.IsActive;
                markdownPlan.CurrentSaleDate = updatedMarkdownPlan.CurrentSaleDate;
                markdownPlan.SaleEnded = updatedMarkdownPlan.SaleEnded;
                markdownPlan.IntermediateCompleted = updatedMarkdownPlan.IntermediateCompleted;
                markdownPlan.InitialCompleted = updatedMarkdownPlan.InitialCompleted;
                markdownPlan.InitialInventory = updatedMarkdownPlan.InitialInventory;

                //context.SaveChanges();

                context.SaveChanges();

                await context.SaveChangesAsync();

                return Results.Ok();

            });


            // DELETE markdown plan by ID
            app.MapDelete("/markdownplan/{id}", (StoreContext context, int id) =>
            {
                var markdownPlan = context.MarkdownPlans.Find(id);
                if (markdownPlan == null) return Results.NotFound();

                context.MarkdownPlans.Remove(markdownPlan);
                context.SaveChanges();
                return Results.Ok();
            });
        }
    }
}
