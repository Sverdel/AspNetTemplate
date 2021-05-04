using System.Linq;
using AspNetTemplate.Core.Model;
using AspNetTemplate.Infrastructure.DataAccess;
using AutoBogus;
using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace AspNetTemplate.IntegrationTests.Setup
{   
    public class TestServerFixture : WebApplicationFactory<Startup>
    {
        private const string _environment = "Test";
        
        public IAutoFaker AutoFaker { get; } = AutoBogus.AutoFaker.Create();
        public Faker Faker { get; } = new Faker();
        
        public ServiceProvider ServiceProvider { get; private set; }

        protected override IHostBuilder CreateHostBuilder()
        {
            return base.CreateHostBuilder()
                .UseEnvironment(_environment);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .ConfigureTestServices(services =>
                {
                    // Don't run IHostedServices when running as a test
                    services.RemoveAll(typeof(IHostedService));

                    ServiceProvider = services.BuildServiceProvider();
                    var factory = ServiceProvider.GetRequiredService<IDbContextFactory<AspNetTemplateContext>>();
                    using var context = factory.CreateDbContext();
                    InitTestDatabase(context);
                });
        }

        private void InitTestDatabase(DbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var entities = AutoFaker.Generate<Entity>(10).ToList();
            context.Set<Entity>().AddRange(entities);
            context.SaveChanges();
        }
    }
}