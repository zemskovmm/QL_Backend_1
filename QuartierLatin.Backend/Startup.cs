using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.AppStateRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Models;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Constants;
using QuartierLatin.Backend.Application.Infrastructure.Database;
using QuartierLatin.Backend.Application.Infrastructure.Database.AppDbContextSeed;
using QuartierLatin.Backend.Config;
using QuartierLatin.Backend.Managers.Auth;
using QuartierLatin.Backend.Storages;
using QuartierLatin.Backend.Storages.Cache;
using QuartierLatin.Backend.Utils;
using QuartierLatin.Backend.Utils.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.Web.Sitemap;

namespace QuartierLatin.Backend
{
    public class Startup
    {
        public static IServiceProvider AppServices { get; private set; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static bool IntegrationTestMode { get; set; }

        private DatabaseConfig DatabaseConfig => Configuration.GetSection("Database").Get<DatabaseConfig>();
        private SitemapConfig SitemapConfig => Configuration.GetSection("Sitemap").Get<SitemapConfig>();

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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins().AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
            });

            services.AddControllers();
            AutoRegisterByTypeName(services);
            services.AddSingleton<UserAuthManager>();
            services.AddSingleton<ISitemapGenerator, SitemapGenerator>();
            services.AddSingleton<ISitemapIndexGenerator, SitemapIndexGenerator>();
            services.AddSingleton<SitemapGeneratorForLinks>();
            services.AddSingleton<GlobalSettingsCache<JObject>>();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return Task.CompletedTask;
                    };
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                })
                .AddCookie(CookieAuthenticationPortal.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/api/portal/login/";
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return Task.CompletedTask;
                    };
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                });

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerFileOperationFilter>();
                options.MapType(typeof(IFormFile), () => new OpenApiSchema() { Type = "file", Format = "binary" });
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(Roles.Admin);
                });
            });

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
                services.AddSingleton<IConfirmationService, EmailConfirmationService>();
            else
                services.AddSingleton<IConfirmationService, NoopEmailConfirmationService>();

            if (Configuration.GetSection("AzureAD").Exists())
                services.AddSingleton<IAzureAdClient, AzureAdClient>();
            else
                services.AddSingleton<IAzureAdClient, NoopAzureAdClient>();

            services.Configure<CallRequestConfig>(Configuration.GetSection("CallRequest"));
            services.Configure<BaseFilterOrderConfig>(Configuration.GetSection("BaseFilterOrder"));
            services.Configure<SitemapConfig>(Configuration.GetSection("Sitemap"));

            services.AddRazorPages().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var roleRepository = app.ApplicationServices.GetRequiredService<IRoleRepository>();
            var userAuthManager = app.ApplicationServices.GetRequiredService<UserAuthManager>();
            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials()
            );

            void StaticFiles()
            {
                var devJsRoot = Path.Combine(Directory.GetCurrentDirectory(), "webapp");
                if (Directory.Exists(devJsRoot))
                {
                    var dist = Path.GetFullPath(Path.Combine(devJsRoot, "build"));
                    Directory.CreateDirectory(dist);
                    app.UseStaticFiles(new StaticFileOptions
                        {FileProvider = new PhysicalFileProvider(dist), RequestPath = ""});
                }

                var contentRoot = env.ContentRootPath;
                var sitemapDirectory = Path.Combine(contentRoot, SitemapConfig.Directory);
                
                if (!Directory.Exists(sitemapDirectory))
                    Directory.CreateDirectory(sitemapDirectory);

                app.UseStaticFiles(new StaticFileOptions
                    { FileProvider = new PhysicalFileProvider(sitemapDirectory), RequestPath = "/sitemaps" });

                app.UseStaticFiles(); // for wwwroot
            }
            
            StaticFiles();
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            const string swaggerApiPrefix = "/api/swagger"; 
            app.Map(swaggerApiPrefix, swaggerApp =>
            {
                swaggerApp.Use((ctx, next) =>
                {
                    ctx.Request.PathBase = "/";
                    ctx.Request.Path = "/swagger" + ctx.Request.Path;
                    return next();
                });
                swaggerApp.UseSwagger(c =>
                {
                    c.SerializeAsV2 = true;
                });
                swaggerApp.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "QuartierLatin API");
                });
            });

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
            AppDbContextSeed.Seed(app.ApplicationServices.GetRequiredService<AppDbContextManager>());
            
            AppServices = app.ApplicationServices;

            void StartUpdater() => Task.Run(async () => {
                var appStateRepo = AppServices.GetService<IAppStateEntryRepository>();
                var sitemapUpdater = AppServices.GetService<SitemapGeneratorForLinks>();
                while (true)
                {
                    var dates = await appStateRepo.GetLastUpdateAndLastChangeDatesAsync();

                    if (dates.lastUpdate < dates.lastChange)
                        sitemapUpdater.GenerateSitemaps();
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }

            });

            StartUpdater();
        }
    }
}