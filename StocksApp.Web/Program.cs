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
using StocksApp.Web.Middlewares;
using StocksApp.Web.StartupExtensions;


var builder = WebApplication.CreateBuilder(args);

builder.ConfigigureServices();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandlingMiddleware();
    app.UseExceptionHandler("/error");
}

app.UseSerilogRequestLogging();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

Log.Information("Hello, World!");
app.Run();
