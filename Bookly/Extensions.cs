using Interfaces;
using Bookly.Business_logic.Services;
using Bookly.Data.Repository;
using Business_logic.Helpers;
using Business_logic.Services;

namespace Bookly.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection Services)
        {
            Services.AddScoped<IGoalRepository, GoalRepository>();
            Services.AddScoped<IShelfRepository, ShelfRepository>();
            Services.AddScoped<IUserRepository, UserRepository>();
            Services.AddScoped<IBookRepository, BookRepository>();
            Services.AddScoped<IReviewRepository, ReviewRepository>();
            Services.AddScoped<IRatingRepostiory, RatingRepository>();

            return Services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection Services)
        {
            Services.AddScoped<IUserServices, UserServices>();
            Services.AddScoped<IBookServices, BookServices>();
            Services.AddScoped<IShelfServices, ShelfServices>();
            Services.AddScoped<IGoalServices, GoalServices>();
            Services.AddScoped<IRandomServices, RandomServices>();
            Services.AddScoped<IReviewServices, ReviewServices>();
            Services.AddScoped<IRatingServices, RatingServices>();
            Services.AddScoped<IBookDetailsService, BookDetailsService>();
            Services.AddScoped<IPasswordHelper, PasswordHelper>();
            Services.AddScoped<ISessionHelper, SessionHelper>();
            Services.AddScoped<IUserValidation, UserValidation>();

            Services.AddTransient<IEmailSender, EmailSender>();

            Services.AddScoped<IEmailService, EmailService>();
            Services.AddHostedService<EmailService>();

            return Services;
        }
    }
}
