using Microsoft.EntityFrameworkCore;
using OsrsTool.Domain.Models;
using OsrsTool.Domain.Interfaces;
using OsrsTool.Infrastructure.Data;
using OsrsTool.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// DbContext voor elk type dat je wilt gebruiken
builder.Services.AddDbContext<GenericDbContext<Item>>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IRepository<Item>, Repository<Item>>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GenericDbContext<Item>>();
    db.Database.Migrate();
}


app.Run();
