using Serilog;
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

Log.Information("Hello, World!");
app.Run();
