using Bookly.Data.InterfacesRepo;
using Bookly.Business_logic.InterfacesServices;
using Bookly.Data.Repository;
using Bookly.Business_logic.Services;
using Business_logic.Services;
using Business_logic.InterfacesServices;
using Business_logic.Mappers;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAutoMapper(typeof(UserProfile));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IBookRepository,BookRepository>();
            builder.Services.AddScoped<IBookServices, BookServices>();
            builder.Services.AddScoped<IShelfRepository, ShelfRepository>();
            builder.Services.AddScoped<IShelfServices, ShelfServices>();
            builder.Services.AddScoped<IGoalRepository, GoalRepository>();
            builder.Services.AddScoped<IGoalServices, GoalServices>();
            builder.Services.AddScoped<IRandomServices, RandomServices>();

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
