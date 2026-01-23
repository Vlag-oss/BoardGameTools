using Microsoft.Extensions.Configuration;

namespace BoardGameTools.Infrastructure.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
        {
            var value = configuration[key];
            if (string.IsNullOrEmpty(value))
                throw new InvalidOperationException($"La configuration {key} n'existe pas ou n'a pas de valeur");

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"La configuraiton {key} ne peut pas être converti dans le type {typeof(T).Name}", ex);
            }
        }

        public static T GetRequiredValue<T>(this IConfigurationSection section, string key)
        {
            var value = section[key];
            if (string.IsNullOrEmpty(value))
                throw new InvalidOperationException($"La configuration {section.Path}:{key} n'existe pas ou n'a pas de valeur");

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"La configuraiton {section.Path}:{key} ne peut pas être converti dans le type {typeof(T).Name}", ex);
            }
        }
    }
}
