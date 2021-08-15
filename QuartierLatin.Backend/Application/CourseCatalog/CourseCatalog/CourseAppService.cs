﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.Interfaces.CourseCatalog.CourseCatalog;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.CourseCatalogModels.CoursesModels;
using QuartierLatin.Backend.Models.Repositories.AppStateRepository;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Models.Repositories.CourseCatalogRepository.CourseRepository;

namespace QuartierLatin.Backend.Application.CourseCatalog.CourseCatalog
{
    public class CourseAppService : ICourseAppService
    {
        private readonly ICourseCatalogRepository _courseCatalogRepository;
        private readonly IAppStateEntryRepository _appStateEntryRepository;
        private readonly ICommonTraitRepository _commonTraitRepository;

        public CourseAppService(ICourseCatalogRepository courseCatalogRepository, IAppStateEntryRepository appStateEntryRepository,
            ICommonTraitRepository commonTraitRepository)
        {
            _courseCatalogRepository = courseCatalogRepository;
            _appStateEntryRepository = appStateEntryRepository;
            _commonTraitRepository = commonTraitRepository;
        }

        public async Task<List<(Course course, Dictionary<int, CourseLanguage> courseLanguage)>> GetCourseListAsync()
        {
            return await _courseCatalogRepository.GetCourseListAsync();
        }

        public async Task<int> CreateCourseAsync(int schoolId, int? imageId)
        {
            var id = await _courseCatalogRepository.CreateCourseAsync(schoolId, imageId);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
            return id;
        }

        public async Task CreateCourseLanguageListAsync(List<CourseLanguage> courseLanguage)
        {
            await _courseCatalogRepository.CreateCourseLanguageListAsync(courseLanguage);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
        }

        public async Task<(Course course, Dictionary<int, CourseLanguage> schoolLanguage)> GetCourseByIdAsync(int id)
        {
            return await _courseCatalogRepository.GetCourseByIdAsync(id);
        }

        public async Task UpdateCourseByIdAsync(int id, int schoolId, int? imageId)
        {
            await _courseCatalogRepository.UpdateCourseByIdAsync(id, schoolId, imageId);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
        }

        public async Task UpdateCourseLanguageByIdAsync(int id, string htmlDescription, int languageId, string name, string url, JObject? metadata)
        {
            await _courseCatalogRepository.CreateOrUpdateCourseLanguageByIdAsync(id, htmlDescription, languageId, name, url, metadata);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
        }

        public async Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByCourseIdsAsync(IEnumerable<int> courseIds)
        {
            return await _commonTraitRepository.GetCommonTraitListByCourseIdsAsync(courseIds);
        }
    }
}
