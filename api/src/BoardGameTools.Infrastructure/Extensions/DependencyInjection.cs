using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Infrastructure.Persistence.Contexts;
using BoardGameTools.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BoardGameTools.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            var cs = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(cs))
                throw new InvalidOperationException("Le paramètre pour la ConnectionString n'a pas de valeur ou n'existe pas");

            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.UseNpgsql(cs);
            });

            services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
            services.AddScoped<IEmailSender, BrevoEmailSender>();
            services.AddScoped<IEmailTemplate, EmailTemplate>();
        }
    }
}
