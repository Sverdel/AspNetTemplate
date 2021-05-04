using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetTemplate.Extensions
{
    public static class ServiceCollectionExtensions
    {
    
        
        public static IServiceCollection RegisterHostedServices(this IServiceCollection services)
        {
            return services;
        }
        
        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            return services.AddScoped<IMapper, Mapper>();
        }
    }
}