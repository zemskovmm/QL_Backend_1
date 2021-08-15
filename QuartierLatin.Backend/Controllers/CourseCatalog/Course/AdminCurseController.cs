using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces.CourseCatalog.CourseCatalog;
using QuartierLatin.Backend.Dto.CourseCatalogDto.Course;
using QuartierLatin.Backend.Models.CourseCatalogModels.CoursesModels;
using QuartierLatin.Backend.Models.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RemoteUi;

namespace QuartierLatin.Backend.Controllers.courseCatalog.course
{
    public class SchoolAdminDtoResponse
    {
        public JObject Definition { get; set; }
        public CourseAdminDto Value { get; set; }
    }
    
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/courses")]
    public class AdmincourseController : Controller
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ICourseAppService _courseAppService;
        private readonly JObject _definition;


        public AdmincourseController(ILanguageRepository languageRepository, ICourseAppService courseAppService)
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
            var courseId = await _courseAppService.CreateCourseAsync(courseDto.SchoolId, courseDto.ImageId);
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var courseLanguage = courseDto.Languages.Select(course => new CourseLanguage
            {
                CourseId = courseId,
                Description = course.Value.HtmlDescription,
                Name = course.Value.Name,
                Url = course.Value.Url,
                LanguageId = language.FirstOrDefault(language => language.Value == course.Key).Key,
                Metadata = course.Value.Metadata is null ? null : course.Value.Metadata.ToString() 
            }).ToList();

            await _courseAppService.CreateCourseLanguageListAsync(courseLanguage);

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
            await _courseAppService.UpdateCourseByIdAsync(id, courseDto.SchoolId, courseDto.ImageId);
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
