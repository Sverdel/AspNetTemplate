using AspNetTemplate.Core.Services;
using AspNetTemplate.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetTemplate.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration,
            string connectionStringName = "Db")
        {
            return services
                .AddPooledDbContextFactory<AspNetTemplateContext>(o =>
                {
                    o.UseNpgsql(configuration.GetConnectionString(connectionStringName), builder =>
                            builder
                                .CommandTimeout(120)
                                .MigrationsHistoryTable("__migrations_history", "migrations"))
                        .UseSnakeCaseNamingConvention();
                })
                .AddSingleton<IStorage, AspNetTemplateStorage>();
        }
    }
}