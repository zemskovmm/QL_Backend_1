using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace QuartierLatin.Backend.Tests.Infrastructure
{
    public class TestBase
    {
        private static bool _appStarted;
        private static ExceptionDispatchInfo _appStartFailed;
        private static readonly object AppStartLock = new();
        private static readonly string AppUrl = $"http://127.0.0.1:{GetFreePort()}";

        protected TestBase() => AppStart();

        protected string RpcUri => AppUrl.TrimEnd('/') + "/tsrpc";

        protected T GetService<T>() => (T) Startup.AppServices.GetRequiredService(typeof (T));
        
        private static void AppStartCore()
        {
            Startup.IntegrationTestMode = true;
            while (!File.Exists("QuartierLatin.Backend.sln"))
                Directory.SetCurrentDirectory("..");

            var testRootPath = Path.GetFullPath("QuartierLatin.Backend.Tests/test-temp-root/root");
            if (Directory.Exists(testRootPath))
                Directory.Delete(testRootPath, true);
            Directory.CreateDirectory(testRootPath);

            var customConfig = Path.GetFullPath("QuartierLatin.Backend.Tests/config.json");
            var testConfig = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(customConfig));
            Directory.SetCurrentDirectory("QuartierLatin.Backend.Tests");

            var conns = testConfig["ConnectionString"].Value<string>();
            using (var conn = new NpgsqlConnection(conns))
            {
                conn.Open();
                foreach (var sql in new[] {"drop schema if exists public cascade;", "create schema public;"})
                {
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }

            var host = Program
                .CreateHostBuilder(new string[0])
                .ConfigureAppConfiguration((hb, cb) =>
                {
                    cb.Sources.Clear();
                    cb.AddJsonFile(customConfig);
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls(AppUrl)
                .Build();

            host.Start();
        }
        
        private static void AppStart()
        {
            lock (AppStartLock)
            {
                if (_appStartFailed != null)
                    _appStartFailed.Throw();
                else if (_appStarted)
                    return;
                try
                {
                    AppStartCore();
                }
                catch (Exception e)
                {
                    _appStartFailed = ExceptionDispatchInfo.Capture(e);
                    throw;
                }

                _appStarted = true;
            }
        }

        private static int GetFreePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var ep = (IPEndPoint) listener.LocalEndpoint;
            var port = ep.Port;
            listener.Stop();
            return port;
        }
    }
}