using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ZoomitTest.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
         public static TSettings AddConfig<TSettings>(this IServiceCollection services, IConfiguration configuration) where TSettings : class, new()
        {
            return services.AddConfig<TSettings>(configuration, delegate
            {
            });
        }

        public static TSettings AddConfig<TSettings>(this IServiceCollection services, IConfiguration configuration, Action<BinderOptions> configureOptions) where TSettings : class, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException("services");
            }
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            TSettings val = configuration.Get<TSettings>(configureOptions);
            services.TryAddSingleton(val);
            return val;
        }
    }
}