using System;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Config;
using QuartierLatin.Backend.Models;
using FluentMigrator.Exceptions;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.PostgreSQL;
using LinqToDB.DataProvider.SqlServer;
using Microsoft.Extensions.Options;
using QuartierLatin.Backend.Models.CatalogModels;

namespace QuartierLatin.Backend.Database
{
    public class AppDbContext : DataConnection
    {
        public AppDbContext(string connectionString, IDataProvider provider) : base(provider, connectionString)
        {
        }

        public ITable<Admin> Admins => GetTable<Admin>();
        public ITable<AdminRole> AdminRoles => GetTable<AdminRole>();
        public ITable<Blob> Blobs => GetTable<Blob>();
        public ITable<Page> Pages => GetTable<Page>();
        public ITable<Language> Languages => GetTable<Language>();
        public ITable<PageRoot> PageRoots => GetTable<PageRoot>();
        public ITable<GlobalSetting> GlobalSettings => GetTable<GlobalSetting>();
        public ITable<University> Universities => GetTable<University>();
        public ITable<UniversityLanguage> UniversityLanguages => GetTable<UniversityLanguage>();
        public ITable<SpecialtyCategory> SpecialtyCategories => GetTable<SpecialtyCategory>();
        public ITable<Specialty> Specialties => GetTable<Specialty>();
        public ITable<UniversitySpecialty> UniversitySpecialties => GetTable<UniversitySpecialty>();
        public ITable<CommonTrait> CommonTraits => GetTable<CommonTrait>();
        public ITable<CommonTraitType> CommonTraitTypes => GetTable<CommonTraitType>();
        public ITable<CommonTraitsToUniversity> CommonTraitsToUniversities => GetTable<CommonTraitsToUniversity>();
        public ITable<CommonTraitTypesForEntity> CommonTraitTypesForEntities => GetTable<CommonTraitTypesForEntity>();
    }

    public interface IAppDbConnectionFactory
    {
        public AppDbContext GetConnection();
    }

    public class AppDbConnectionFactory : IAppDbConnectionFactory
    {
        private readonly IOptions<DatabaseConfig> _config;

        public AppDbConnectionFactory(IOptions<DatabaseConfig> config)
        {
            _config = config;
        }

        public AppDbContext GetConnection()
        {
            return _config.Value.Type switch
            {
                DatabaseType.SqlServer => new AppDbContext(ConnectionString(), MssqlProvider()),
                DatabaseType.Pgsql => new AppDbContext(ConnectionString(), PgsqlProvider()),
                _ => throw new DatabaseOperationNotSupportedException(
                    $"{nameof(AppDbConnectionFactory)} can't find available db type connection")
            };
        }

        private static SqlServerDataProvider MssqlProvider()
        {
            return new
                ("app", SqlServerVersion.v2017, SqlServerProvider.MicrosoftDataSqlClient);
        }

        private static PostgreSQLDataProvider PgsqlProvider()
        {
            return new();
        }

        private string ConnectionString()
        {
            return _config.Value.ConnectionString;
        }
    }

    public class AppDbContextManager : DbContextManagerBase<AppDbContext>
    {
        public AppDbContextManager(IAppDbConnectionFactory factory) : base(factory.GetConnection)
        {
        }
    }

    public class DbContextManagerBase<TContext> where TContext : DataConnection
    {
        private readonly Func<TContext> _factory;

        public DbContextManagerBase(Func<TContext> factory)
        {
            _factory = factory;
        }

        public void Exec(Action<TContext> cb)
        {
            using (var ctx = _factory())
            {
                cb(ctx);
            }
        }

        public T Exec<T>(Func<TContext, T> cb)
        {
            using (var ctx = _factory())
            {
                var rv = cb(ctx);
                if (rv is IQueryable)
                    throw new InvalidOperationException("IQueryable leak detected");
                return rv;
            }
        }

        public async Task<T> ExecAsync<T>(Func<TContext, Task<T>> cb)
        {
            using (var ctx = _factory())
            {
                return await cb(ctx);
            }
        }

        public async Task ExecAsync(Func<TContext, Task> cb)
        {
            using (var ctx = _factory())
            {
                await cb(ctx);
            }
        }
    }
}