using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces.CourseCatalog.CourseCatalog;
using QuartierLatin.Backend.Dto.CourseCatalogDto.Course;
using QuartierLatin.Backend.Models.CourseCatalogModels.CoursesModels;
using QuartierLatin.Backend.Models.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers.courseCatalog.course
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/courses")]
    public class AdmincourseController : Controller
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ICourseAppService _courseAppService;

        public AdmincourseController(ILanguageRepository languageRepository, ICourseAppService courseAppService)
        {
            _languageRepository = languageRepository;
            _courseAppService = courseAppService;
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
                Languages = course.courseLanguage.ToDictionary(school => language[school.Key],
                    school => new CourseLanguageAdminDto
                    {
                        Name = school.Value.Name,
                        HtmlDescription = school.Value.Description,
                        Url = school.Value.Url
                    })
            }).ToList();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseAdminDto courseDto)
        {
            var courseId = await _courseAppService.CreateCourseAsync(courseDto.SchoolId);
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var courseLanguage = courseDto.Languages.Select(course => new CourseLanguage
            {
                CourseId = courseId,
                Description = course.Value.HtmlDescription,
                Name = course.Value.Name,
                Url = course.Value.Url,
                LanguageId = language.FirstOrDefault(language => language.Value == course.Key).Key
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
                Languages = course.schoolLanguage.ToDictionary(course => language[course.Key],
                    course => new CourseLanguageAdminDto
                    {
                        Name = course.Value.Name,
                        HtmlDescription = course.Value.Description,
                        Url = course.Value.Url
                    })
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourseById([FromBody] CourseAdminDto courseDto, int id)
        {
            await _courseAppService.UpdateCourseByIdAsync(id, courseDto.SchoolId);
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            foreach (var courseLanguage in courseDto.Languages)
            {
                var languageId = language.FirstOrDefault(language => language.Value == courseLanguage.Key).Key;
                await _courseAppService.UpdateCourseLanguageByIdAsync(id,
                    courseLanguage.Value.HtmlDescription,
                    languageId,
                    courseLanguage.Value.Name,
                    courseLanguage.Value.Url);
            }

            return Ok(new object());
        }
    }
}
