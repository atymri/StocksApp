using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using StocksApp.Entities;
using StocksApp.Repositories;
using StocksApp.RepositoryContracts;
using StocksApp.ServiceContracts;
using StocksApp.Services;
using StocksApp.Web;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddScoped<IFinnhubService, FinnhubServic>();
builder.Services.AddScoped<IStocksService, StocksService>();
builder.Services.AddScoped<IFinnHubRepository, FinnHubRepository>();
builder.Services.AddScoped<IStocksRepository, StocskRepository>();

string connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("connection string is null!!");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        path:"logs/log-.txt", 
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        retainedFileCountLimit: 30)
    .WriteTo.Seq("http://localhost:5341", restrictedToMinimumLevel: LogEventLevel.Information)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddHttpClient();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseSerilogRequestLogging();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

Log.Information("Hello, World!");
app.Run();
