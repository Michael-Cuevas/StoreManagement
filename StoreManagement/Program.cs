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

var isDevelopment = builder.Environment.IsDevelopment();
var allowedOrigins = isDevelopment ?
                     new[] { "http://localhost:7126" } :
                     new[] { "https://storeclient.azurewebsites.net" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("DynamicPolicy",
        builder =>
        {
            builder.WithMethods("GET", "POST", "PUT", "DELETE")
                   .WithHeaders("Content-Type", "Authorization", "X-Requested-With")
                   .WithOrigins(allowedOrigins);
        });
});

var app = builder.Build();

app.UseCors("DynamicPolicy");


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

