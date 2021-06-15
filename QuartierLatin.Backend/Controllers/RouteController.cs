using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Dto.CommonTraitDto;
using QuartierLatin.Backend.Dto.courseCatalogDto.School.ModuleDto;
using QuartierLatin.Backend.Dto.CourseCatalogDto.Course.ModuleDto;
using QuartierLatin.Backend.Dto.UniversityDto;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Models.Repositories.courseCatalogRepository.SchoolRepository;
using QuartierLatin.Backend.Models.Repositories.CourseCatalogRepository.CourseRepository;
using QuartierLatin.Backend.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers
{
    public class RouteController : Controller
    {
        private readonly ICommonTraitAppService _commonTraitAppService;
        private readonly ILanguageRepository _languageRepository;
        private readonly IRouteAppService _routeAppService;
        private readonly ICommonTraitTypeAppService _traitTypeAppService;
        private readonly IUniversityAppService _universityAppService;
        private readonly ISpecialtyAppService _specialtyAppService;
        private readonly IDegreeRepository _degreeRepository;
        private readonly ISchoolCatalogRepository _schoolCatalogRepository;
        private readonly ICourseCatalogRepository _courseCatalogRepository;

        public RouteController(IRouteAppService routeAppService, IUniversityAppService universityAppService,
            ILanguageRepository languageRepository, ICommonTraitAppService commonTraitAppService,
            ICommonTraitTypeAppService traitTypeAppService, ISpecialtyAppService specialtyAppService,
            IDegreeRepository degreeRepository, ISchoolCatalogRepository schoolCatalogRepository,
            ICourseCatalogRepository courseCatalogRepository)
        {
            _routeAppService = routeAppService;
            _universityAppService = universityAppService;
            _languageRepository = languageRepository;
            _commonTraitAppService = commonTraitAppService;
            _traitTypeAppService = traitTypeAppService;
            _specialtyAppService = specialtyAppService;
            _degreeRepository = degreeRepository;
            _schoolCatalogRepository = schoolCatalogRepository;
            _courseCatalogRepository = courseCatalogRepository;
        }

        [HttpGet("/api/route/{lang}/{**route}")]
        public async Task<IActionResult> GetPage(string lang, string route)
        {
            var routeResponse = await _routeAppService.GetPageByUrlAsync(lang, route);

            if (routeResponse is null)
                return BadRequest();

            return Ok(routeResponse);
        }

        [Authorize]
        [HttpGet("/admin/api/route/{**route}")]
        public async Task<IActionResult> GetPageAdmin(string route)
        {
            var routeResponse = await _routeAppService.GetPageByUrlAdminAsync(route);

            if (routeResponse is null)
                return BadRequest();

            return Ok(routeResponse);
        }

        [HttpGet("/api/route/{lang}/university/{**url}")]
        public async Task<IActionResult> GetUniversity(string lang, string url)
        {
            var languageIds = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var languageId = languageIds.FirstOrDefault(language => language.Value == lang).Key;

            var university = await _universityAppService.GetUniversityByUrlWithLanguage(languageId, url);

            var urls = university.Item2.ToDictionary(
                university => languageIds[university.Key],
                university => university.Value.Url);

            var traitsType = await _traitTypeAppService.GetTraitTypesWithIndetifierAsync();

            var traits = new Dictionary<string, List<CommonTraitLanguageDto>>();

            foreach (var traitType in traitsType)
            {
                var commonTraits = await _commonTraitAppService.GetTraitOfTypesByTypeIdAndUniversityIdAsync(traitType.Id, university.Item1.Id);

                traits.Add(traitType.Identifier, commonTraits.Select(trait => new CommonTraitLanguageDto
                {
                    Id = trait.Id,
                    IconBlobId = trait.IconBlobId,
                    Identifier = trait.Identifier,
                    Name = trait.Names[lang]
                }).ToList());
            }

            var specialtiesUniversity =
                await _specialtyAppService.GetSpecialtiesUniversityByUniversityId(university.Item1.Id);

            var degreesForUniversity = await _degreeRepository.GetDegreesForUniversity(university.Item1.Id);
            
            var universityTraits = new UniversityModuleTraitsDto
            {
                NamedTraits = traits,
                UniversitySpecialties = specialtiesUniversity.Select(specialties => new UniversitySpecialtiesDto
                {
                    Name = specialties.Names.GetSuitableName(lang)
                }).ToList(),
                UniversityDegrees = degreesForUniversity.Select(x=>new UniversityDegreeDto()
                {
                    Name = x.degree.Names.GetSuitableName(lang),
                    CostFrom = CostGroup.GetCostGroup(x.costGroup).from,
                    CostTo = CostGroup.GetCostGroup(x.costGroup).to
                }).ToList()
            };

            var module = new UniversityModuleDto
            {
                Title = university.Item2[languageId].Name,
                DescriptionHtml = university.Item2[languageId].Description,
                FoundationYear = university.Item1.FoundationYear,
                Traits = universityTraits
            };

            var response = new RouteDto<UniversityModuleDto>("university", urls, module, "university");

            return Ok(response);
        }

        [HttpGet("/api/route/{lang}/school/{**url}")]
        public async Task<IActionResult> GetSchool(string lang, string url)
        {
            var languageIds = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var languageId = languageIds.FirstOrDefault(language => language.Value == lang).Key;

            var school = await _schoolCatalogRepository.GetSchoolByUrlWithLanguageAsync(languageId, url);

            var urls = school.schoolLanguage.ToDictionary(
                school => languageIds[school.Key],
                school => school.Value.Url);

            var traitsType = await _traitTypeAppService.GetTraitTypesWithIndetifierAsync();

            var traits = new Dictionary<string, List<CommonTraitLanguageDto>>();

            foreach (var traitType in traitsType)
            {
                var commonTraits = await _commonTraitAppService.GetTraitOfTypesByTypeIdAndSchoolIdAsync(traitType.Id, school.school.Id);

                traits.Add(traitType.Identifier, commonTraits.Select(trait => new CommonTraitLanguageDto
                {
                    Id = trait.Id,
                    IconBlobId = trait.IconBlobId,
                    Identifier = trait.Identifier,
                    Name = trait.Names[lang]
                }).ToList());
            }

            var schoolTraits = new SchoolModuleTraitsDto
            {
                NamedTraits = traits,
            };

            var module = new SchoolModuleDto
            {
                Title = school.Item2[languageId].Name,
                DescriptionHtml = school.Item2[languageId].Description,
                FoundationYear = school.Item1.FoundationYear,
                Traits = schoolTraits
            };

            var response = new RouteDto<SchoolModuleDto>("school", urls, module, "school");

            return Ok(response);
        }

        [HttpGet("/api/route/{lang}/course/{**url}")]
        public async Task<IActionResult> GetCourse(string lang, string url)
        {
            var languageIds = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var languageId = languageIds.FirstOrDefault(language => language.Value == lang).Key;

            var course = await _courseCatalogRepository.GetCourseByUrlWithLanguageAsync(languageId, url);

            var urls = course.courseLanguage.ToDictionary(
                course => languageIds[course.Key],
                course => course.Value.Url);

            var traitsType = await _traitTypeAppService.GetTraitTypesWithIndetifierAsync();

            var traits = new Dictionary<string, List<CommonTraitLanguageDto>>();

            foreach (var traitType in traitsType)
            {
                var commonTraits = await _commonTraitAppService.GetTraitOfTypesByTypeIdAndCourseIdAsync(traitType.Id, course.course.Id);

                traits.Add(traitType.Identifier, commonTraits.Select(trait => new CommonTraitLanguageDto
                {
                    Id = trait.Id,
                    IconBlobId = trait.IconBlobId,
                    Identifier = trait.Identifier,
                    Name = trait.Names[lang]
                }).ToList());
            }

            var courseTraits = new CourseModuleTraitsDto()
            {
                NamedTraits = traits,
            };

            var module = new CourseModuleDto
            {
                Title = course.Item2[languageId].Name,
                DescriptionHtml = course.Item2[languageId].Description,
                SchoolId = course.Item1.SchoolId,
                Traits = courseTraits
            };

            var response = new RouteDto<CourseModuleDto>("course", urls, module, "course");

            return Ok(response);
        }
    }
}