using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.Shared.Models;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddDbContext<StoreContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
builder.Services.AddScoped<StoreContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials()
                   .SetIsOriginAllowed(origin => true); //TODO, make more specific policy
        });
});

var app = builder.Build();

app.UseCors("AllowAll");


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<StoreContext>();
    dbContext.Database.Migrate();
    //TODO, if i switch to postgres, use the following instead
    // services.AddDbContext<StoreContext>(options => options.UseNpgsql("YourPostgreSQLConnectionStringHere"));

    dbContext.SeedData();
}


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
app.MapGet("/markdownplans", (StoreContext context) =>
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
    int saleProgress = markdown.EndDate.DayNumber - datum.SalesDate.DayNumber;
    float saleProgressPercent = 1- (saleProgress / saleLength);

    if (!markdown.IntermediateCompleted && saleProgressPercent >= .5)
    {
        product.Price = product.Price * (1-(markdown.IntermidiateReduction *.01m));
        markdown.IntermediateCompleted = true;
        //context.Entry(markdown).State = EntityState.Modified;

    }
    else if(salesDatum.SalesDate == markdown.EndDate.AddDays(-1))
    {
        product.Price = product.Price *(1- (markdown.FinalReduction * .01m));
        markdown.IsActive = true;
        //context.Entry(markdown).State = EntityState.Modified;

    }
    else if(salesDatum.SalesDate == markdown.EndDate)
    {
        markdown.SaleEnded = true;
        //context.Entry(markdown).State = EntityState.Modified;
    }


    inventory.Quantity = inventory.Quantity - salesDatum.TotalSold;

    await context.SaveChangesAsync();

    return Results.NoContent();

});

app.Run();

