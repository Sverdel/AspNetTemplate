using System.Reflection;
using System.Text.Json.Serialization;
using AspNetTemplate.Core.Handlers;
using AspNetTemplate.Extensions;
using AspNetTemplate.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Prometheus;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspNetTemplate
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var assemblies = new []
            {
                typeof(HelloCommandHandler).GetTypeInfo().Assembly
            };
            
            services.AddMapping()
                .AddMediatR(assemblies)
                .RegisterHostedServices()
                .RegisterInfrastructureServices(_configuration)
                .AddSwaggerGen(c =>
                {
                    c.CustomOperationIds(apiDesc
                        => apiDesc.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : null);
                    c.SwaggerDoc("v1", new OpenApiInfo {Title = _environment.ApplicationName, Version = "v1"});
                })
                .AddResponseCompression()
                .AddRouting(options =>
                {
                    options.LowercaseUrls = true;
                    options.LowercaseQueryStrings = true;
                })
                .AddControllers()
                .AddJsonOptions(options => 
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting()
                .UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.All
                })
                .UseMetricServer()
                .UseHttpMetrics()
                .UseSwagger()
                .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", _environment.ApplicationName))
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.Map("/ping", c => c.Response.WriteAsync("pong"));
                });
        }
    }
}