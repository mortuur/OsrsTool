using Microsoft.EntityFrameworkCore;
using OsrsTool.Domain.Interfaces;
using OsrsTool.Infrastructure.Data;
using OsrsTool.Infrastructure.Repositories;
using OsrsTool.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// DbContext voor elk type dat je wilt gebruiken
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Services httpclient
builder.Services.AddHttpClient<IOsrsApiService, OsrsApiService>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "OsrsTool/1.0 (contact: github.com/mortuur)");
});
// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
// services
builder.Services.AddScoped<IItemService, Itemservice>();

// Hosted background service
builder.Services.AddHostedService<OsrsApiBackgroundService>();
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

app.Run();
