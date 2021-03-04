using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using QuartierLatin.Admin.Auth;
using QuartierLatin.Admin.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace QuartierLatin.Admin
{
    public class Program
    {
        private static void ConfigureServices(IServiceCollection services)
        {
            ServiceRunner.RegisterServices(services);
        }

        public static async Task Main(string[] args)
        {
            if (args.FirstOrDefault() == "encodepass")
            {
                string pass;
                if (args.Length > 1)
                {
                    pass = args[1];
                }
                else
                {
                    Console.WriteLine("Enter:");
                    pass = Console.ReadLine();
                }

                Console.WriteLine(PasswordToolkit.EncodeSshaPassword(pass));
                return;
            }
            
            var built = CreateHostBuilder(args).Build();

            if (args.Contains("--Services")) ServiceRunner.StartServices(built.Services);
            await built.RunAsync();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureAppConfiguration((hb, cb) =>
                {
                    cb.Sources.Clear();
                    cb.AddJsonFile("config.defaults.json")
                        .AddJsonFile("config.local.json", true)
                        .AddJsonFile("/apps/quartier-admin/wd/quartier-config.json", true)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);
                }).UseStartup<Startup>();

            if (args.Contains("--Services"))
                hostBuilder
                    .ConfigureServices(ConfigureServices)
                    .UseServer(new EmptyServer());

            if (args.Contains("--Web") || args.Length is 0)
                hostBuilder
                    .UseKestrel()
                    .UseUrls("http://0.0.0.0:12321");

            return hostBuilder;
        }

        // Used if Recognizer mode only
        private class EmptyServer : IServer
        {
            public IFeatureCollection Features => new FeatureCollection();

            public Task StartAsync<TContext>(IHttpApplication<TContext> application,
                CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            public void Dispose()
            {
            }
        }
    }
}