using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Dto.CommonTraitDto;
using QuartierLatin.Backend.Dto.UniversityDto;
using QuartierLatin.Backend.Models.Repositories;
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

        public RouteController(IRouteAppService routeAppService, IUniversityAppService universityAppService,
            ILanguageRepository languageRepository, ICommonTraitAppService commonTraitAppService,
            ICommonTraitTypeAppService traitTypeAppService, ISpecialtyAppService specialtyAppService)
        {
            _routeAppService = routeAppService;
            _universityAppService = universityAppService;
            _languageRepository = languageRepository;
            _commonTraitAppService = commonTraitAppService;
            _traitTypeAppService = traitTypeAppService;
            _specialtyAppService = specialtyAppService;
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

            var universityTraits = new UniversityModuleTraitsDto
            {
                NamedTraits = traits,
                UniversitySpecialties = specialtiesUniversity.Select(specialties => new UniversitySpecialtiesDto
                {
                    Name = specialties.Item1.Names[lang], Cost = specialties.Item2
                }).ToList()
            };

            var module = new UniversityModuleDto
            {
                Title = university.Item2[languageId].Name,
                DescriptionHtml = university.Item2[languageId].Description,
                Traits = universityTraits
            };

            var response = new RouteDto<UniversityModuleDto>("university", urls, module, "university");

            return Ok(response);
        }
    }
}