using Microsoft.EntityFrameworkCore;
using FitnessProManager.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// 1. DATABASE CONNECTION
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 2. IMPORTANT: Allows the server to show your HTML file
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

// 3. IMPORTANT: Makes sure index.html loads when you open the site
app.MapFallbackToFile("index.html");

app.Run();