using BabyfloServer.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
using BabyfloServer.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Define the CORS Policy EXACTLY like this
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

// 2. CRITICAL: UseCors MUST come BEFORE MapControllers
app.UseRouting(); 
app.UseCors("AllowAll"); 

app.MapControllers();

app.Run();
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite("Data Source=babyflo.db"));

var app = builder.Build();

app.UseCors("AllowAll"); // Enable CORS here
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.EnsureCreated(); // Automatically creates tables if missing
}

app.Run();
