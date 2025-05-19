using Bookly.Business_logic.InterfacesServices;
using Bookly.Extensions;
using Bookly.Filters;
using Bookly.Mappers;
using Business_logic.Mappers;
using Hangfire;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });

            // Add services to the container
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAutoMapper(typeof(UserModelMapper).Assembly, typeof(ShelfMapper).Assembly);

            builder.Services.RegisterRepositories();
            builder.Services.RegisterServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
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
