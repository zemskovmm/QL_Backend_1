﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Dto.TraitTypeDto;
using QuartierLatin.Backend.Dto.UniversityDto;
using QuartierLatin.Backend.Models.Repositories;

namespace QuartierLatin.Backend.Controllers
{
    public class RouteController : Controller
    {
        private readonly ICommonTraitAppService _commonTraitAppService;
        private readonly ILanguageRepository _languageRepository;
        private readonly IRouteAppService _routeAppService;
        private readonly ICommonTraitTypeAppService _traitTypeAppService;
        private readonly IUniversityAppService _universityAppService;

        public RouteController(IRouteAppService routeAppService, IUniversityAppService universityAppService,
            ILanguageRepository languageRepository, ICommonTraitAppService commonTraitAppService,
            ICommonTraitTypeAppService traitTypeAppService)
        {
            _routeAppService = routeAppService;
            _universityAppService = universityAppService;
            _languageRepository = languageRepository;
            _commonTraitAppService = commonTraitAppService;
            _traitTypeAppService = traitTypeAppService;
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
            var university = await _universityAppService.GetUniversityByUrl(url);

            var urls = university.Item2.ToDictionary(
                university => _languageRepository.GetLanguageShortNameAsync(university.Key)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult(),
                university => university.Value.Url);

            var traitsType = await _traitTypeAppService.GetTraitTypesAsync();

            var traitTypeCityId = traitsType.FirstOrDefault(trait => trait.Identifier == "city").Id;
            var traitTypeDegreeId = traitsType.FirstOrDefault(trait => trait.Identifier == "degree").Id;

            var cityTraits = await _commonTraitAppService.GetTraitOfTypesByTypeIdAndUniversityIdAsync(traitTypeCityId, university.Item1.Id);
            var degreeTraits = await _commonTraitAppService.GetTraitOfTypesByTypeIdAndUniversityIdAsync(traitTypeDegreeId, university.Item1.Id);

            var instructionsLanguage =
                await _universityAppService.GetUniversityLanguageInstructionByUniversityId(university.Item1.Id);
            var specialtiesUniversity =
                await _universityAppService.GetSpecialtiesUniversityByUniversityId(university.Item1.Id);

            var universityTraits = new UniversityModuleTraitsDto
            {
                Cities = cityTraits.Select(city => new TraitTypeLanguageDto
                {
                    Id = city.Id, Identifier = city.Identifier, Name = city.Names[lang], IconBlobId = city.IconBlobId
                }).ToList(),
                Degrees = degreeTraits.Select(degree => new TraitTypeLanguageDto
                {
                    Id = degree.Id,
                    IconBlobId = degree.IconBlobId,
                    Identifier = degree.Identifier,
                    Name = degree.Names[lang]
                }).ToList(),
                InstructionLanguage = instructionsLanguage.Select(instructions =>
                    new UniversityInstructionLanguageDto
                    {
                        Name = _languageRepository.GetLanguageNameById(instructions.LanguageId)
                            .ConfigureAwait(false)
                            .GetAwaiter()
                            .GetResult()
                    }).ToList(),
                UniversitySpecialties = specialtiesUniversity.Select(specialties => new UniversitySpecialtiesDto
                {
                    Name = specialties.Item1.Names[lang], Cost = specialties.Item2
                }).ToList()
            };

            var languageId = await _languageRepository.GetLanguageIdByShortNameAsync(lang);

            var module = new UniversityModuleDto
            {
                Title = university.Item2[languageId].Name,
                DescriptionHtml = university.Item2[languageId].Description,
                Traits = universityTraits
            };

            var response = new RouteDto<UniversityModuleDto>(urls, module, "university");

            return Ok(response);
        }
    }
}