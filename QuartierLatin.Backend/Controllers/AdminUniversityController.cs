using System.Collections.Generic;
using QuartierLatin.Backend.Dto.UniversityDto;
using QuartierLatin.Backend.Dto.UniversityDto.GetUniversityListDto;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.Catalog;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;

namespace QuartierLatin.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/universities")]
    public class AdminUniversityController : Controller
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IUniversityAppService _universityAppService;
        private readonly IUniversityGalleryAppService _universityGalleryAppService;

        public AdminUniversityController(IUniversityAppService universityAppService,
            ILanguageRepository languageRepository, IUniversityGalleryAppService universityGalleryAppService)
        {
            _universityAppService = universityAppService;
            _languageRepository = languageRepository;
            _universityGalleryAppService = universityGalleryAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUniversity()
        {
            var universityList = await _universityAppService.GetUniversityListAsync();
            var languageIds = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var response = new List<UniversityListDto>();

            foreach (var university in universityList)
            {
                var gallery = await _universityGalleryAppService.GetGalleryToUniversityAsync(university.university.Id);

                response.Add(new UniversityListDto
                {
                    Id = university.university.Id,
                    FoundationYear = university.university.FoundationYear,
                    MinimumAge = 18,
                    Website = "/",
                    Languages = university.universityLanguage.ToDictionary(university =>
                        languageIds.FirstOrDefault(lang => lang.Key == university.Key).Value, university => new UniversityLanguageDto
                    {
                        Name = university.Value.Name,
                        HtmlDescription = university.Value.Description,
                        Url = university.Value.Url,
                        Metadata = university.Value.Metadata is null ? null : JObject.Parse(university.Value.Metadata)
                    }),
                    LogoId = university.university.LogoId,
                    BannerId = university.university.BannerId,
                    GalleryList = gallery
                });
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUniversity([FromBody] CreateUniversityDtoAdmin university)
        {
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var universityLanguage = university.Languages.Select(university => new UniversityLanguage
            {
                Description = university.Value.HtmlDescription,
                Name = university.Value.Name,
                Url = university.Value.Url,
                Metadata = university.Value.Metadata is null ? null : university.Value.Metadata.ToString(),
                LanguageId = language.FirstOrDefault(language => language.Value == university.Key).Key
            }).ToList();

            var universityId = await _universityAppService.CreateUniversityAsync(university.FoundationYear, university.LogoId, university.BannerId, universityLanguage);

            return Ok(new {id = universityId});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUniversityById(int id)
        {
            var university = await _universityAppService.GetUniversityByIdAsync(id);
            var languageIds = await _languageRepository.GetLanguageIdWithShortNameAsync();
            var gallery = await _universityGalleryAppService.GetGalleryToUniversityAsync(id);


            var response = new UniversityDto
            {
                FoundationYear = university.university.FoundationYear,
                Languages = university.universityLanguage.ToDictionary(university =>
                        languageIds.FirstOrDefault(lang => lang.Key == university.Key).Value,
                    university => new UniversityLanguageDto
                {
                    Name = university.Value.Name,
                    HtmlDescription = university.Value.Description,
                    Metadata = university.Value.Metadata is null ? null : JObject.Parse(university.Value.Metadata),
                    Url = university.Value.Url
                }),
                LogoId = university.university.LogoId,
                BannerId = university.university.BannerId,
                GalleryList = gallery
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUniversityById([FromBody] CreateUniversityDtoAdmin universityDto, int id)
        {
            await _universityAppService.UpdateUniversityByIdAsync(id, universityDto.FoundationYear, universityDto.LogoId, universityDto.BannerId);
            var languageIds = await _languageRepository.GetLanguageIdWithShortNameAsync();

            foreach (var universityLanguage in universityDto.Languages)
            {
                var languageId = languageIds.FirstOrDefault(lang => lang.Value == universityLanguage.Key).Key;
                await _universityAppService.UpdateUniversityLanguageByIdAsync(id,
                    universityLanguage.Value.HtmlDescription,
                    languageId,
                    universityLanguage.Value.Name,
                    universityLanguage.Value.Url, universityLanguage.Value.Metadata);
            }

            return Ok(new object());
        }
    }
}