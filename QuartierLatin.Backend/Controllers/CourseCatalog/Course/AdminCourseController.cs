using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.CourseCatalog.CourseCatalog;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.CoursesModels;
using QuartierLatin.Backend.Dto.CourseCatalogDto.Course;
using RemoteUi;

namespace QuartierLatin.Backend.Controllers.CourseCatalog.Course
{
    public class SchoolAdminDtoResponse
    {
        public JObject Definition { get; set; }
        public CourseAdminDto Value { get; set; }
    }
    
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/courses")]
    public class AdminCourseController : Controller
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ICourseAppService _courseAppService;
        private readonly JObject _definition;


        public AdminCourseController(ILanguageRepository languageRepository, ICourseAppService courseAppService)
        {
            var noFields = new IExtraRemoteUiField[0];

            _languageRepository = languageRepository;
            _courseAppService = courseAppService;
            _definition = new RemoteUiBuilder(typeof(CourseAdminDto), noFields, null, new CamelCaseNamingStrategy())
                .Register(typeof(CourseAdminDto), noFields)
                .Register(typeof(Dictionary<string, CourseLanguageAdminDto>), noFields)
                .Register(typeof(CourseLanguageAdminDto), noFields)
                .Build(null);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourse()
        {
            var courseList = await _courseAppService.GetCourseListAsync();
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();
            
            var response = courseList.Select(course => new CourseListAdminDto()
            {
                Id = course.course.Id,
                SchoolId = course.course.SchoolId,
                ImageId = course.course.ImageId,
                Price = course.course.Price,
                Languages = course.courseLanguage.ToDictionary(school => language[school.Key],
                    course => new CourseLanguageAdminDto
                    {
                        Name = course.Value.Name,
                        HtmlDescription = course.Value.Description,
                        Url = course.Value.Url,
                        Metadata = course.Value.Metadata is null ? null : JObject.Parse(course.Value.Metadata)
                    })
            }).ToList();

            return Ok(response);
        }
        
        [HttpGet("definition")]
        public IActionResult GetDefinition() => Ok(_definition);

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseAdminDto courseDto)
        {
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var courseLanguage = courseDto.Languages.Select(course => new CourseLanguage
            {
                Description = course.Value.HtmlDescription,
                Name = course.Value.Name,
                Url = course.Value.Url,
                LanguageId = language.FirstOrDefault(language => language.Value == course.Key).Key,
                Metadata = course.Value.Metadata is null ? null : course.Value.Metadata.ToString() 
            }).ToList();

            var courseId = await _courseAppService.CreateCourseAsync(courseDto.SchoolId, courseDto.ImageId, courseDto.Price, courseLanguage);

            return Ok(new { id = courseId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _courseAppService.GetCourseByIdAsync(id);
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var response = new CourseAdminDto
            {
                SchoolId = course.course.SchoolId,
                ImageId = course.course.ImageId,
                Price = course.course.Price,
                Languages = course.schoolLanguage.ToDictionary(course => language[course.Key],
                    course => new CourseLanguageAdminDto
                    {
                        Name = course.Value.Name,
                        HtmlDescription = course.Value.Description,
                        Url = course.Value.Url,
                        Metadata = course.Value.Metadata is null ? null : JObject.Parse(course.Value.Metadata)
                    })
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourseById([FromBody] CourseAdminDto courseDto, int id)
        {
            await _courseAppService.UpdateCourseByIdAsync(id, courseDto.SchoolId, courseDto.ImageId, courseDto.Price);
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            foreach (var courseLanguage in courseDto.Languages)
            {
                var languageId = language.FirstOrDefault(language => language.Value == courseLanguage.Key).Key;
                await _courseAppService.UpdateCourseLanguageByIdAsync(id,
                    courseLanguage.Value.HtmlDescription,
                    languageId,
                    courseLanguage.Value.Name,
                    courseLanguage.Value.Url, courseLanguage.Value.Metadata);
            }

            return Ok(new object());
        }
    }
}
