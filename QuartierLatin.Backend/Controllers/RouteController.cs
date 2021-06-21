using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Dto.CommonTraitDto;
using QuartierLatin.Backend.Dto.UniversityDto;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.Repositories;
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
        private readonly IUniversityGalleryAppService _universityGalleryAppService;

        public RouteController(IRouteAppService routeAppService, IUniversityAppService universityAppService,
            ILanguageRepository languageRepository, ICommonTraitAppService commonTraitAppService,
            ICommonTraitTypeAppService traitTypeAppService, ISpecialtyAppService specialtyAppService,
            IDegreeRepository degreeRepository, IUniversityGalleryAppService universityGalleryAppService)
        {
            _routeAppService = routeAppService;
            _universityAppService = universityAppService;
            _languageRepository = languageRepository;
            _commonTraitAppService = commonTraitAppService;
            _traitTypeAppService = traitTypeAppService;
            _specialtyAppService = specialtyAppService;
            _degreeRepository = degreeRepository;
            _universityGalleryAppService = universityGalleryAppService;
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

            var gallery = await _universityGalleryAppService.GetGalleryToUniversityAsync(university.university.Id);

            var module = new UniversityModuleDto
            {
                Title = university.Item2[languageId].Name,
                DescriptionHtml = university.Item2[languageId].Description,
                FoundationYear = university.Item1.FoundationYear,
                Traits = universityTraits,
                LogoId = university.university.LogoId,
                BannerId = university.university.BannerId,
                GalleryList = gallery
            };

            var response = new RouteDto<UniversityModuleDto>("university", urls, module, "university", university.Item2[languageId].Name);

            return Ok(response);
        }
    }
}