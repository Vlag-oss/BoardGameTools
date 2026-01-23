using BoardGameTools.Application.Common.Behaviours;
using BoardGameTools.Application.Services.Passwords;
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

            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        }
    }
}
