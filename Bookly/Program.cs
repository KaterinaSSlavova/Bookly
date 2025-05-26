using Bookly.Extensions;
using Bookly.Filters;
using Bookly.Mappers;
using Business_logic.Mappers;
using EFDataLayer.DBContext;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Exceptions;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });



            // Add services to the container
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAutoMapper(typeof(UserModelMapper).Assembly, typeof(ShelfMapper).Assembly);

            builder.Services.AddDbContext<BooklyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Server=DESKTOP-GPBCRNQ;Database=BooklyDB;Trusted_Connection=True; TrustServerCertificate=True;")));

            builder.Services.RegisterRepositories();
            builder.Services.RegisterServices();
            builder.Host.UseSerilog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=LogIn}/{id?}");

            app.Run();
        }
    }
}
