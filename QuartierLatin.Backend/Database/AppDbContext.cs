using FluentMigrator.Exceptions;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.PostgreSQL;
using LinqToDB.DataProvider.SqlServer;
using Microsoft.Extensions.Options;
using QuartierLatin.Backend.Config;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.AppStateModels;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.CourseCatalogModels.CoursesModels;
using QuartierLatin.Backend.Models.CourseCatalogModels.SchoolModels;
using QuartierLatin.Backend.Models.FolderModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models.HousingModels;
using QuartierLatin.Backend.Models.ImageStandardSizeModels;

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
        public ITable<Degree> Degrees => GetTable<Degree>();
        public ITable<UniversityDegree> UniversityDegrees => GetTable<UniversityDegree>();
        public ITable<StorageFolder> StorageFolders => GetTable<StorageFolder>();
        public ITable<UniversityGallery> UniversityGalleries => GetTable<UniversityGallery>();
        public ITable<CommonTraitToSchool> CommonTraitToSchools => GetTable<CommonTraitToSchool>();
        public ITable<CommonTraitToCourse> CommonTraitToCourses => GetTable<CommonTraitToCourse>();
        public ITable<School> Schools => GetTable<School>();
        public ITable<SchoolLanguages> SchoolLanguages => GetTable<SchoolLanguages>();
        public ITable<Course> Courses => GetTable<Course>();
        public ITable<CourseLanguage> CourseLanguages => GetTable<CourseLanguage>();
        public ITable<AppStateEntry> AppStateEntries => GetTable<AppStateEntry>();
        public ITable<CommonTraitsToPage> CommonTraitsToPages => GetTable<CommonTraitsToPage>();
        public ITable<ImageStandardSize> ImageStandardSizes => GetTable<ImageStandardSize>();
        public ITable<Housing> Housings => GetTable<Housing>();
        public ITable<HousingLanguage> HousingLanguages => GetTable<HousingLanguage>();
        public ITable<HousingAccommodationType> HousingAccommodationTypes => GetTable<HousingAccommodationType>();
        public ITable<CommonTraitToHousing> CommonTraitToHousings => GetTable<CommonTraitToHousing>();
        public ITable<CommonTraitToHousingAccommodationType> CommonTraitToHousingAccommodationTypes =>
            GetTable<CommonTraitToHousingAccommodationType>();
        public ITable<HousingGallery> HousingGalleries => GetTable<HousingGallery>();
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