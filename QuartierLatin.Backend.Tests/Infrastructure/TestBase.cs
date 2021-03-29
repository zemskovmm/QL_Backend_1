using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using QuartierLatin.Backend.Models.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.Text;

namespace QuartierLatin.Backend.Tests.Infrastructure
{
    public class TestBase
    {
        private static bool _appStarted;
        private static ExceptionDispatchInfo _appStartFailed;
        private static readonly object AppStartLock = new();
        private static readonly string AppUrl = $"http://127.0.0.1:{GetFreePort()}";
        private static string _assetPath;


        protected TestBase() => AppStart();

        protected string RpcUri => AppUrl.TrimEnd('/') + "/tsrpc";

        protected T GetService<T>() => (T) Startup.AppServices.GetRequiredService(typeof (T));

        protected static string GetAssetPath() => _assetPath;

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

            var enviroment = System.Environment.CurrentDirectory;
            _assetPath = Path.Combine(enviroment, "Assets");

            var conns = testConfig["Database"]["ConnectionString"].Value<string>();
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
                .ConfigureLogging(l => l.AddDebug())
                .ConfigureAppConfiguration((hb, cb) =>
                {
                    cb.Sources.Clear();
                    cb.AddJsonFile(customConfig);
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls(AppUrl)
                .Build();

            host.Start();
            LangIds = host.Services.GetRequiredService<ILanguageRepository>().GetLanguageListAsync().Result
                .ToDictionary(x => x.LanguageShortName, x => x.Id);
            SendApiRequest<object>(AdminClient, "/api/admin/auth/login",
                new {Username = "user@example.com", Password = "123321"});
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

        static HttpClient AnonClient = new HttpClient();

        private static HttpClient AdminClient = new HttpClient(new HttpClientHandler
            {CookieContainer = new CookieContainer()});

        protected static T SendAdminRequest<T>(string url, object data, HttpMethod method = null,
            Action<HttpRequestMessage> configure = null, bool isFile = false) =>
            SendApiRequest<T>(AdminClient, url, data, method, configure, isFile);

        protected static T SendAnonRequest<T>(string url, object data, HttpMethod method = null,
            Action<HttpRequestMessage> configure = null, bool isFile = false) =>
            SendApiRequest<T>(AnonClient, url, data, method, configure, isFile);

        protected static T SendApiRequest<T>(HttpClient client, string url, object data, HttpMethod method = null, Action<HttpRequestMessage> configure = null, bool isFile = false)
        {
            url = AppUrl + "/" + url.TrimStart('/');
            var msg = new  HttpRequestMessage(method ?? (data == null ? HttpMethod.Get :  HttpMethod.Post), url);
            if (data != null)
            {
                if (data.GetType() == typeof(MultipartFormDataContent))
                {
                    msg.Content = (MultipartFormDataContent)data;
                }
                else
                {
                    msg.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                }
            }

            var respString = "";

            configure?.Invoke(msg);
            var resp = AnonClient.Send(msg);
            
            if (resp.IsSuccessStatusCode)
            {
                if (isFile is true)
                {
                    return (T)(object)Convert.ToBase64String(resp.Content.ReadAsByteArrayAsync().ConfigureAwait(false).GetAwaiter().GetResult());
                }

                respString = resp.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(respString);
            }

            throw new Exception($"Status code {resp.StatusCode}:\n{respString}");
        }

        protected static Dictionary<string, int> LangIds;
    }
}