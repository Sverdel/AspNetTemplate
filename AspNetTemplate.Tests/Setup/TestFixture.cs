using System;
using System.Reflection;
using AspNetTemplate.Infrastructure;
using AutoBogus;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace AspNetTemplate.Tests.Setup
{
    public class TestFixture
    {
        public IAutoFaker AutoFaker => LazyContainer.AutoFaker;

        public IServiceProvider ServiceProvider => ServiceCollection.BuildServiceProvider();

        private IServiceCollection ServiceCollection => LazyContainer.InternalServiceCollection;

        [UsedImplicitly]
        private static class LazyContainer
        {
            internal static readonly IServiceCollection InternalServiceCollection;

            internal static readonly IAutoFaker AutoFaker = AutoBogus.AutoFaker.Create();

            static LazyContainer()
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                var env = new Mock<IWebHostEnvironment>();
                env.Setup(x => x.EnvironmentName).Returns("Test");

                InternalServiceCollection = new ServiceCollection();

                var assemblies = new[]
                {
                    typeof(Startup).GetTypeInfo().Assembly,
                };

                InternalServiceCollection
                    .AddMediatR(assemblies)
                    .RegisterInfrastructureServices(configuration)
                    .AddRouting(options =>
                    {
                        options.LowercaseUrls = true;
                        options.LowercaseQueryStrings = true;
                    })
                    .AddControllers();

                InitMocks();

                InternalServiceCollection.AddLogging();

                InternalServiceCollection.AddScoped<IHttpContextAccessor>(sp => new HttpContextAccessor
                {
                    HttpContext = new DefaultHttpContext
                    {
                        RequestServices = sp
                    }
                });
            }

            private static void InitMocks()
	        {
            
            }
        }
    }
}