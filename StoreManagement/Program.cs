using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.Models;
using System.Linq;

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

    context.SaveChanges();

    await context.SaveChangesAsync();

    return Results.NoContent();

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


app.Run();

