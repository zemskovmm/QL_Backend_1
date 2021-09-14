using System;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Dto.CommonTraitDto;
using QuartierLatin.Backend.Dto.CourseCatalogDto.Course.ModuleDto;
using QuartierLatin.Backend.Dto.CourseCatalogDto.School.ModuleDto;
using QuartierLatin.Backend.Dto.UniversityDto;
using QuartierLatin.Backend.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.CourseRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.SchoolRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.Catalog;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.HousingServices;
using QuartierLatin.Backend.Application.ApplicationCore.Models;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.CoursesModels;
using QuartierLatin.Backend.Dto.CourseCatalogDto.RouteDto;
using QuartierLatin.Backend.Dto.HousingCatalogDto.RouteDto;
using QuartierLatin.Backend.Dto.RouteDto;

namespace QuartierLatin.Backend.Controllers
{
    [Route("/api/route")]
    public class RouteController : Controller
    {
        private readonly ICommonTraitAppService _commonTraitAppService;
        private readonly ILanguageRepository _languageRepository;
        private readonly IRouteAppService _routeAppService;
        private readonly ICommonTraitTypeAppService _traitTypeAppService;
        private readonly IUniversityAppService _universityAppService;
        private readonly ISpecialtyAppService _specialtyAppService;
        private readonly IDegreeRepository _degreeRepository;
        private readonly IUniversityGalleryAppService _universityGalleryAppService;
        private readonly ISchoolCatalogRepository _schoolCatalogRepository;
        private readonly ICourseCatalogRepository _courseCatalogRepository;
        private readonly IHousingAppService _housingAppService;
        private readonly IHousingGalleryAppService _housingGalleryAppService;
        private readonly IHousingAccommodationTypeAppService _housingAccommodationTypeAppService;

        public RouteController(IRouteAppService routeAppService, IUniversityAppService universityAppService,
            ILanguageRepository languageRepository, ICommonTraitAppService commonTraitAppService,
            ICommonTraitTypeAppService traitTypeAppService, ISpecialtyAppService specialtyAppService,
            IDegreeRepository degreeRepository, IUniversityGalleryAppService universityGalleryAppService, 
            ISchoolCatalogRepository schoolCatalogRepository, ICourseCatalogRepository courseCatalogRepository,
            IHousingAppService housingAppService, IHousingGalleryAppService housingGalleryAppService,
            IHousingAccommodationTypeAppService housingAccommodationTypeAppService)
        {
            _routeAppService = routeAppService;
            _universityAppService = universityAppService;
            _languageRepository = languageRepository;
            _commonTraitAppService = commonTraitAppService;
            _traitTypeAppService = traitTypeAppService;
            _specialtyAppService = specialtyAppService;
            _degreeRepository = degreeRepository;
            _universityGalleryAppService = universityGalleryAppService;
            _schoolCatalogRepository = schoolCatalogRepository;
            _courseCatalogRepository = courseCatalogRepository;
            _housingAppService = housingAppService;
            _housingGalleryAppService = housingGalleryAppService;
            _housingAccommodationTypeAppService = housingAccommodationTypeAppService;
        }

        [HttpGet("{lang}/{**route}")]
        public async Task<IActionResult> GetPage(string lang, string route)
        {
            var routeResponse = await _routeAppService.GetPageByUrlAsync(lang, route);

            if (routeResponse is null)
                return BadRequest();

            return Ok(routeResponse);
        }

        [Authorize]
        [HttpGet("{**route}")]
        public async Task<IActionResult> GetPageAdmin(string route)
        {
            var routeResponse = await _routeAppService.GetPageByUrlAdminAsync(route);

            if (routeResponse is null)
                return BadRequest();

            return Ok(routeResponse);
        }

        [HttpGet("{lang}/university/{**url}")]
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

            var gallery = await _universityGalleryAppService.GetGalleryToUniversityAsync(university.university.Id);

            var module = new UniversityModuleDto
            {
                Title = university.Item2[languageId].Name,
                DescriptionHtml = university.Item2[languageId].Description,
                FoundationYear = university.Item1.FoundationYear,
                Traits = universityTraits,
                LogoId = university.university.LogoId,
                BannerId = university.university.BannerId,
                GalleryList = gallery,
                Metadata = university.universityLanguage[languageId].Metadata is null ? null : JObject.Parse(university.universityLanguage[languageId].Metadata),
            };

            var response = new RouteDto<UniversityModuleDto>("university", urls, module, "university", university.Item2[languageId].Name);

            return Ok(response);
        }

        [HttpGet("{lang}/school/{**url}")]
        public async Task<IActionResult> GetSchool(string lang, string url)
        {
            var moduleAndUrls = await GetSchoolModuleDto(lang, url);

            var response = new RouteDto<SchoolModuleDto>("school", moduleAndUrls.urls, moduleAndUrls.schoolModule, "school", moduleAndUrls.schoolModule.Title);

            return Ok(response);
        }

        [HttpGet("{lang}/{schoolUrl}/courses/{**url}")]
        public async Task<IActionResult> GetSchoolAndCourse(string lang, string schoolUrl, string url)
        {
            var courseModuleAndUrls = await GetCourseModuleDtoWithUrls(lang, url);

            var schoolModuleAndUrls = await GetSchoolModuleDto(lang, schoolUrl);

            var module = new SchoolCourseModuleDto
            {
                Course = courseModuleAndUrls.courseModule,
                School = schoolModuleAndUrls.schoolModule
            };

            var languageIds = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var urls = languageIds.Where(langs => schoolModuleAndUrls.urls.ContainsKey(langs.Value) && courseModuleAndUrls.urls.ContainsKey(langs.Value))
                .ToDictionary(langs => langs.Value, 
                    langs => String.Format("{0}/courses/{1}", schoolModuleAndUrls.urls[langs.Value], courseModuleAndUrls.urls[langs.Value]));

            var response = new RouteDto<SchoolCourseModuleDto>(null, urls, module, "schoolAndCourse", schoolModuleAndUrls.schoolModule.Title + ", " + courseModuleAndUrls.courseModule.Title);

            return Ok(response);
        }

        [HttpGet("{lang}/{schoolUrl}/courses/")]
        public async Task<IActionResult> GetSchoolAndCourseList(string lang, string schoolUrl)
        {
            var languageIds = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var schoolModuleAndUrls = await GetSchoolModuleDto(lang, schoolUrl);

            var courseModuleList = await GetCourseModuleDtoList(lang, schoolModuleAndUrls.schoolId, languageIds);

            var module = new SchoolCourseListModuleDto()
            {
                Courses = courseModuleList,
                School = schoolModuleAndUrls.schoolModule
            };

            var urls = languageIds.Where(langs => schoolModuleAndUrls.urls.ContainsKey(langs.Value))
                .ToDictionary(langs => langs.Value,
                    langs => String.Format("{0}/courses/", schoolModuleAndUrls.urls[langs.Value]));

            var response = new RouteDto<SchoolCourseListModuleDto>(null, urls, module, "schoolAndCourseList", schoolModuleAndUrls.schoolModule.Title);

            return Ok(response);
        }

        [HttpGet("{lang}/course/{**url}")]
        public async Task<IActionResult> GetCourse(string lang, string url)
        {
            var moduleAndUrls = await GetCourseModuleDtoWithUrls(lang, url);

            var response = new RouteDto<CourseModuleDto>("course", moduleAndUrls.urls, moduleAndUrls.courseModule, "course", moduleAndUrls.courseModule.Title);

            return Ok(response);
        }

        [HttpGet("{lang}/housing/{**url}")]
        public async Task<IActionResult> GetHousing(string lang, string url)
        {
            var moduleAndUrls = await GetHousingModuleAsync(lang, url);

            var response = new RouteDto<HousingModuleDto>("housing", moduleAndUrls.urls, moduleAndUrls.housingModule, "housing", moduleAndUrls.housingModule.Title);

            return Ok(response);
        }

        private async Task<(HousingModuleDto housingModule, Dictionary<string, string> urls)> GetHousingModuleAsync(string lang, string url)
        {
            var languageIds = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var languageId = languageIds.FirstOrDefault(language => language.Value == lang).Key;

            var housing = await _housingAppService.GetHousingByUrlWithLanguageAsync(languageId, url);

            var urls = housing.housingLanguage.ToDictionary(
                housing => languageIds[housing.Key],
                housing => housing.Value.Url);

            var traitsType = await _traitTypeAppService.GetTraitTypesWithIndetifierAsync();

            var housingTraits = new Dictionary<string, List<CommonTraitLanguageDto>>();

            foreach (var traitType in traitsType)
            {
                var commonTraitsHousing = await _commonTraitAppService.GetTraitOfTypesByTypeIdAndHousingIdAsync(traitType.Id, housing.housing.Id);

                housingTraits.Add(traitType.Identifier, commonTraitsHousing.Select(trait => new CommonTraitLanguageDto
                {
                    Id = trait.Id,
                    IconBlobId = trait.IconBlobId,
                    Identifier = trait.Identifier,
                    Name = trait.Names[lang]
                }).ToList());
            }

            var housingTrait = new NamedTraitsModuleDto
            {
                NamedTraits = housingTraits,
            };

            var housingGallery = await _housingGalleryAppService.GetGalleryToHousingAsync(housing.housing.Id);

            var housingAccommodation =
                await _housingAccommodationTypeAppService.GetHousingAccommodationTypeListByHousingIdAsync(
                    housing.housing.Id);

            var housingAccommodationIds = housingAccommodation.Select(housingAcc => housingAcc.Id);

            var housingAccommodationTraits = await 
                _housingAccommodationTypeAppService.GetCommonTraitListByHousingAccommodationTypeIdsAsync(housingAccommodationIds);

            var housingAccommodationModule = new List<HousingAccommodationTypeModuleDto>();

            foreach (var housingAccommodationEntity in housingAccommodation)
            {
                var traits = new Dictionary<string, List<CommonTraitLanguageDto>>();

                foreach (var traitType in traitsType)
                {
                    var housingTraitTypedList = housingAccommodationTraits.GetValueOrDefault(housing.housing.Id)
                        ?.Where(type => type.CommonTraitTypeId == traitType.Id);

                    if (housingTraitTypedList is null) continue;

                    var courseTraitList = housingTraitTypedList.Where(trait => 
                            trait.Names.Any(traitNames => !string.IsNullOrEmpty(traitNames.Value)))
                        .Select(courseTrait => new CommonTraitLanguageDto
                    {
                        Id = courseTrait.Id,
                        IconBlobId = courseTrait.IconBlobId,
                        Identifier = courseTrait.Identifier,
                        Name = courseTrait.Names.GetSuitableName(lang)
                    }).ToList();

                    traits.Add(traitType.Identifier, courseTraitList);
                }

                var housingAccommodationTrait = new NamedTraitsModuleDto
                {
                    NamedTraits = traits,
                };

                housingAccommodationModule.Add(new HousingAccommodationTypeModuleDto
                {
                    Square = housingAccommodationEntity.Square,
                    Price = housingAccommodationEntity.Price,
                    Residents = housingAccommodationEntity.Residents,
                    Traits = housingAccommodationTrait
                });
            }

            var module = new HousingModuleDto
            {
                Title = housing.housingLanguage[languageId].Name,
                HtmlDescription = housing.housingLanguage[languageId].Description,
                Price = housing.housing.Price,
                Traits = housingTrait,
                Metadata = housing.housingLanguage[languageId].Metadata is null ? null : JObject.Parse(housing.housingLanguage[languageId].Metadata),
                ImageId = housing.housing.ImageId,
                GalleryList = housingGallery,
                HousingAccommodationTypes = housingAccommodationModule
            };

            return (housingModule: module, urls: urls);
        }

        private async Task<(SchoolModuleDto schoolModule, Dictionary<string, string> urls, int schoolId)> GetSchoolModuleDto(string lang, string url)
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

            var schoolTraits = new NamedTraitsModuleDto
            {
                NamedTraits = traits,
            };

            var module = new SchoolModuleDto
            {
                Title = school.schoolLanguage[languageId].Name,
                DescriptionHtml = school.schoolLanguage[languageId].Description,
                FoundationYear = school.school.FoundationYear,
                Traits = schoolTraits,
                Metadata = school.schoolLanguage[languageId].Metadata is null ? null : JObject.Parse(school.schoolLanguage[languageId].Metadata),
                ImageId = school.school.ImageId
            };

            return (schoolModule: module, urls: urls, schoolId: school.school.Id);
        }

        private async Task<(CourseModuleDto courseModule, Dictionary<string, string> urls)> GetCourseModuleDtoWithUrls(
            string lang, string url)
        {
            var languageIds = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var courseModule = await GetCourseDto(lang, url, languageIds);

            var urls = courseModule.Item2.Item2.ToDictionary(
                course => languageIds[course.Key],
                course => course.Value.Url);

            return (courseModule: courseModule.courseModule, urls: urls);
        }

        private async Task<(CourseModuleDto courseModule, (Course, Dictionary<int, CourseLanguage>))> GetCourseDto(
            string lang, string url, Dictionary<int, string> languageIds)
        {
            var languageId = languageIds.FirstOrDefault(language => language.Value == lang).Key;

            var course = await _courseCatalogRepository.GetCourseByUrlWithLanguageAsync(languageId, url);

            var traitsType = await _traitTypeAppService.GetTraitTypesWithIndetifierAsync();

            var module = await GetCourseModule(traitsType, course, languageIds, lang, languageId);

            return (courseModule: module, course);
        }

        private async Task<CourseModuleDto> GetCourseModule(List<CommonTraitType> traitsType,
            (Course course, Dictionary<int, CourseLanguage> courseLanguage) course,
            Dictionary<int, string> languageIds, string lang, int languageId) 
        {
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

            var courseTraits = new NamedTraitsModuleDto()
            {
                NamedTraits = traits,
            };

            var module = new CourseModuleDto
            {
                Title = course.courseLanguage[languageId].Name,
                DescriptionHtml = course.courseLanguage[languageId].Description,
                SchoolId = course.course.SchoolId,
                Traits = courseTraits,
                Metadata = course.courseLanguage[languageId].Metadata is null ? null : JObject.Parse(course.courseLanguage[languageId].Metadata),
                ImageId = course.course.ImageId
            };

            return module;
        }

        private async Task<List<CourseModuleDto>> GetCourseModuleDtoList(
            string lang, int schoolId, Dictionary<int, string> languageIds)
        {
            var courses = await _courseCatalogRepository.GetCoursesListAsync(schoolId);

            var response = new List<CourseModuleDto>();

            var languageId = languageIds.FirstOrDefault(language => language.Value == lang).Key;

            var traitsType = await _traitTypeAppService.GetTraitTypesWithIndetifierAsync();

            foreach (var course in courses)
            {
                response.Add(await GetCourseModule(traitsType, course, languageIds, lang, languageId));
            }

            return response;
        }
    }
}