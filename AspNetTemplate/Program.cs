using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace AspNetTemplate
{
public static class Program
    {
        private static readonly string _serviceName = Assembly.GetExecutingAssembly().GetName().Name;

        public static async Task Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();

                Log.Information("Starting web host {ServiceName} service", _serviceName);
                await host.RunAsync();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "The {ServiceName} service has failed", _serviceName);
            }
            finally
            {
                Log.Information("The {ServiceName} service has stopped", _serviceName);
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    var environment = context.HostingEnvironment;
                    var path = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                        ? environment.ContentRootPath
                        : Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                    configurationBuilder.SetBasePath(path);
                    configurationBuilder.AddJsonFile("appsettings.json", true, true);
                    configurationBuilder.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true, true);

                    configurationBuilder.AddEnvironmentVariables();
                    context.Configuration = configurationBuilder.Build();
                })
                .ConfigureServices((context, services) => { services.AddSingleton(context.Configuration); })
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    var environment = context.HostingEnvironment;

                    var loggerConfiguration = new LoggerConfiguration()
                        .ReadFrom.Configuration(context.Configuration)
                        .Destructure.AsScalar<byte[]>()
                        .Enrich.WithProperty("Application", environment.ApplicationName)
                        .Enrich.WithProperty("Environment", environment.EnvironmentName);

                    Log.Logger = loggerConfiguration.CreateLogger();
                    loggingBuilder.AddSerilog(Log.Logger, true);
                    Serilog.Debugging.SelfLog.Enable(Console.Error);
                })
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                        {
                            options.Limits.MinRequestBodyDataRate = null;
                        })
                        .CaptureStartupErrors(true)
                        .UseStartup<Startup>();
                });
    }
}