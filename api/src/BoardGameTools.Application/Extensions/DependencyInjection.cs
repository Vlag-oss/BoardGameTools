using BoardGameTools.Application.Common.Behaviours;
using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Application.Common.Services;
using BoardGameTools.Application.Services.Passwords;
using BoardGameTools.Application.Services.Tokens;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BoardGameTools.Application.Extensions
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                cfg.AddOpenBehavior(typeof(ValidatorBehaviour<,>));
            });

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<ITokenService, TokenService>();

            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        }
    }
}
