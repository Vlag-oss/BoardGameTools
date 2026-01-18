using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BoardGameTools.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new InvalidOperationException("la variable d'environnement pour le mot de passe de la base de données n'a pas été trouvée.");
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if(string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Le paramètre pour la ConnectionString n'a pas de valeur ou n'existe pas");

            connectionString = connectionString.Replace("{DB_PASSWORD}", dbPassword);

            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        }
    }
}
