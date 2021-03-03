using CoreRPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using QuartierLatin.Admin.Auth;
using QuartierLatin.Admin.Auth.Requirements;
using QuartierLatin.Admin.Config;
using QuartierLatin.Admin.Database;
using QuartierLatin.Admin.Managers;
using QuartierLatin.Admin.Models;
using QuartierLatin.Admin.Models.Repositories;
using QuartierLatin.Admin.Storages;
using LinqToDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace QuartierLatin.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static bool IntegrationTestMode { get; set; }

        private DatabaseConfig DatabaseConfig => Configuration.GetSection("Database").Get<DatabaseConfig>();

        private void AutoRegisterByTypeName(IServiceCollection services)
        {
            var types = typeof(Startup).Assembly.GetTypes();

            var namedTypes = new Dictionary<string, Type>();
            types.ToList().ForEach(t => namedTypes[t.Name] = t);

            void RegisterRepository(Type repoType)
            {
                services.AddSingleton(repoType, namedTypes["Sql" + repoType.Name.Substring(1)]);
            }

            void RegisterAppService(Type repoType)
            {
                services.AddSingleton(repoType, namedTypes[repoType.Name.Substring(1)]);
            }

            // Auto-register repositories
            types.Where(t => t.IsInterface && t.Name.EndsWith("Repository"))
                .ToList()
                .ForEach(RegisterRepository);

            // Auto-register App services
            types.Where(t => t.IsInterface && t.Name.EndsWith("AppService"))
                .ToList()
                .ForEach(RegisterAppService);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            AutoRegisterByTypeName(services);
            services.AddSingleton<UserAuthManager>();
            services.AddSingleton<BlobManager>();

            var blobType = new BlobConfig();
            Configuration.GetSection("Blob").Bind(blobType);
            if (blobType.Type is BlobTypes.Azure) services.AddSingleton<IBlobFileStorage, AzureBlobStorage>();
            if (blobType.Type is BlobTypes.Local)
                services.AddSingleton<IBlobFileStorage>(new LocalBlobFileStorage(blobType.Local.Path));

            var tokenStorageConfig = new TokenConfiguration();
            Configuration.GetSection("TokenStorage").Bind(tokenStorageConfig);
            if (tokenStorageConfig.Type is TokenStorageType.JWT)
                services.AddSingleton<ITokenStorage, JwtTokenStorage>();
            if (tokenStorageConfig.Type is TokenStorageType.InMemory)
                services.AddSingleton<ITokenStorage, InMemoryTokenStorage>();

            services.Configure<DatabaseConfig>(Configuration.GetSection("Database"));
            services.Configure<TokenConfiguration>(Configuration.GetSection("TokenStorage"));
            services.Configure<BlobConfig>(Configuration.GetSection("Blob"));
            services.Configure<AzureAuthConfiguration>(Configuration.GetSection("AzureAD"));
            services.Configure<EmailOptions>(Configuration.GetSection("Email"));

            services.AddSingleton<IAppDbConnectionFactory, AppDbConnectionFactory>();
            services.AddSingleton<AppDbContextManager>();

            if (Configuration.GetSection("Email").Exists())
                services.AddSingleton<IEmailConfirmationService, EmailConfirmationService>();
            else
                services.AddSingleton<IEmailConfirmationService, NoopEmailConfirmationService>();

            if (Configuration.GetSection("AzureAD").Exists())
                services.AddSingleton<IAzureAdClient, AzureAdClient>();
            else
                services.AddSingleton<IAzureAdClient, NoopAzureAdClient>();

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var roleRepository = app.ApplicationServices.GetRequiredService<IRoleRepository>();
            var userAuthManager = app.ApplicationServices.GetRequiredService<UserAuthManager>();
            var interceptors = new List<IMethodCallInterceptor>
            {
                new RpcRoleInterceptor(roleRepository, userAuthManager)
            };

            void StaticFiles()
            {
                var devJsRoot = Path.Combine(Directory.GetCurrentDirectory(), "webapp");
                if (Directory.Exists(devJsRoot))
                {
                    File.WriteAllText(Path.Combine(devJsRoot, "src", "api.ts"), TsInterop.GenerateTsRpc());
                    var dist = Path.GetFullPath(Path.Combine(devJsRoot, "build"));
                    Directory.CreateDirectory(dist);
                    app.UseStaticFiles(new StaticFileOptions
                        {FileProvider = new PhysicalFileProvider(dist), RequestPath = ""});
                }

                app.UseStaticFiles(); // for wwwroot
            }

            StaticFiles();
            TsInterop.Register(app, interceptors);

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            var notFound = Encoding.UTF8.GetBytes("Not found");
            app.Use((context, next) =>
            {
                if (context.Request.Method.ToUpperInvariant() != "GET"
                    || context.Request.Path
                        .ToString()?
                        .Split('/')
                        .LastOrDefault()?
                        .IndexOf('.') >= 0)
                {
                    context.Response.StatusCode = 404;
                    return context.Response.Body.WriteAsync(notFound, 0, notFound.Length);
                }

                context.Request.Path = "/index.html";
                return next();
            });

            StaticFiles();
            MigrationRunner.MigrateDb(DatabaseConfig.ConnectionString, typeof(Startup).Assembly, DatabaseConfig.Type);
            app.ApplicationServices.GetRequiredService<AppDbContextManager>()
                .Exec(db =>
                {
                    if (!db.Users.Any())
                        db.Users.Insert(() => new User
                        {
                            Email = "user@example.com", PasswordHash = PasswordToolkit.EncodeSshaPassword("123321"),
                            Confirmed = true
                        });
                });
        }
    }
}