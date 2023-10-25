using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.EndPoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();


string connectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING")
                    ?? builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<StoreContext>(options => {
    options.UseSqlServer(connectionString);
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

app.MapProductEndpoints();
app.MapInventoryEndpoints();
app.MapMarkdownPlanEndpoints();
app.MapSalesDatumEndPoints();
app.MapLogicEndPoints();



app.Run();

