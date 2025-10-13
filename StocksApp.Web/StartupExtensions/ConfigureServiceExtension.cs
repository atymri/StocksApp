using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;
using ServiceContracts;
using Services;
using StocksApp.Entities;
using StocksApp.Repositories;
using StocksApp.RepositoryContracts;
using StocksApp.ServiceContracts;
using StocksApp.Services;
using StocksApp.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace StocksApp.Web.StartupExtensions
{
    public static class ConfigureServiceExtension
    {
        public static void ConfigigureServices(this WebApplicationBuilder builder)
        {
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

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 5;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
            });

            Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");


            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(
                    path: "logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    retainedFileCountLimit: 30)
                .WriteTo.Seq("http://localhost:5341", restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();

            builder.Host.UseSerilog();
            builder.Services.AddHttpClient();
        }
    }
}
