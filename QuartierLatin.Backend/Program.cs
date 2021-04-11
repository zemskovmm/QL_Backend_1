using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using QuartierLatin.Backend.Auth;
using QuartierLatin.Backend.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QuartierLatin.Backend.Cmdlets;

namespace QuartierLatin.Backend
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
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
                return 0;
            }


            if (CmdletManager.IsCommand(args))
                return await CmdletManager.Execute(transformedArgs => CreateHostBuilder(true, transformedArgs), args);
            
            var builder = CreateHostBuilder(false, args);
            
            var built = builder
                .UseKestrel()
                .Build(); 
            
            await built.RunAsync();
            return 0;
        }

        public static IWebHostBuilder CreateHostBuilder(bool skipServer, string[] args)
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
            if (!skipServer)
                hostBuilder.UseKestrel();
            return hostBuilder;
        }
    }
}