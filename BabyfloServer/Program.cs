using BabyfloServer.Data;
using Microsoft.EntityFrameworkCore;
using BabyfloServer.Models; // <-- Make sure to include your Models namespace if needed

var builder = WebApplication.CreateBuilder(args);

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite("Data Source=babyflo.db"));

var app = builder.Build();

// --- UPDATE THIS ENTIRE SECTION TO CREATE AND SEED PRODUCTS ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        context.Database.EnsureCreated(); // Creates the .db file and tables automatically

        // Check if there are no products, then add default ones so the frontend has data!
        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product { Name = "Comforting Powder Scent", Price = 150.00, ImageUrl = "https://placehold.co/300" },
                new Product { Name = "Butterfly Blossom", Price = 180.00, ImageUrl = "https://placehold.co/300" },
                new Product { Name = "Citrus Fresh Premium", Price = 200.00, ImageUrl = "https://placehold.co/300" }
            );
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating or seeding the DB.");
    }
}
// --- END OF DATABASE SECTION ---

app.UseRouting();
app.UseCors("AllowAll"); 

app.MapControllers();

app.Run();
