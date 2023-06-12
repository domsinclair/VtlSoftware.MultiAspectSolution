using Metalama.Framework.Aspects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using VtlSoftware.Aspects.Logging21;
using VtlSoftware.Aspects.Polly21;

[assembly: AspectOrder(typeof(RetryAttribute), typeof(TimedLogMethodAttribute))]

namespace VtlSoftware.MultiAspects.ConsoleApp
{
    internal class Program
    {
        #region Private Methods

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .CreateLogger();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(
                    services =>
                    {
                        services.AddTransient<Calculator>();
                        services.AddSingleton<IPolicyFactory, PolicyFactory>();
                    })
                .UseSerilog()
                .Build();

            Log.Logger.Information("Application Starting");

            var svc = ActivatorUtilities.CreateInstance<Calculator>(host.Services);
            svc.Subtract(3, 2);
            svc.Multiply(3, 2);
            svc.Add(3, 2);

            Log.Logger.Information("Application Closing");
            Log.CloseAndFlush();
        }

        #endregion
    }
}