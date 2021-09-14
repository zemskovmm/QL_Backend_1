﻿using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto.CatalogSearchResponseDto;
using QuartierLatin.Backend.Dto.CourseCatalogDto.Course.CatalogDto;
using QuartierLatin.Backend.Tests.Infrastructure;
using QuartierLatin.Backend.Tests.TraitTest.TraitTestsDataSet;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.CourseRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.SchoolRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Tests.CourseCatalogTests.DataSets;
using Xunit;

namespace QuartierLatin.Backend.Tests.CourseCatalogTests.CourseTests.CatalogTests
{
    public class GetCourseCatalogListByFiltersTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Anon_User_Should_Be_Able_To_Get_Course_Catalog_By_Filter_ListAsync(JObject school,
            int foundationYear, JObject course, JObject commonTrait,
            JObject cityTraitType, JObject degreeTraitType,
            JObject filter)
        {
            school["foundationYear"] = foundationYear;
            var resp = SendAdminRequest<JObject>("/api/admin/schools", school);
            var id = int.Parse(resp["id"].ToString());
            var repo = GetService<ISchoolCatalogRepository>();

            var schoolWithLanguageEntity = await repo.GetSchoolByIdAsync(id);

            Assert.Equal(foundationYear, schoolWithLanguageEntity.school.FoundationYear);

            foreach (var schoolLanguage in schoolWithLanguageEntity.schoolLanguage)
            {
                Assert.Equal(schoolLanguage.Value.SchoolId, id);
            }

            course["schoolId"] = id;

            var courseResp = SendAdminRequest<JObject>("/api/admin/courses", course);
            var courseId = int.Parse(courseResp["id"].ToString());
            var courseRepo = GetService<ICourseCatalogRepository>();
            var courseWithLanguageEntity = await courseRepo.GetCourseByIdAsync(courseId);

            Assert.Equal(id, courseWithLanguageEntity.course.SchoolId);

            foreach (var courseLanguage in courseWithLanguageEntity.courseLanguage)
            {
                Assert.Equal(courseLanguage.Value.CourseId, courseId);
            }

            var respTraitCity = SendAdminRequest<JObject>("/api/admin/trait-types", cityTraitType);
            var respTraitDegree = SendAdminRequest<JObject>("/api/admin/trait-types", degreeTraitType);

            var traitCityId = int.Parse(respTraitCity["id"].ToString());
            var traitDegreeId = int.Parse(respTraitDegree["id"].ToString());

            var responseCommonTraitCity =
                SendAdminRequest<JObject>($"/api/admin/traits/of-type/{traitCityId}", commonTrait);
            var responseCommonTraitDegree =
                SendAdminRequest<JObject>($"/api/admin/traits/of-type/{traitDegreeId}", commonTrait);

            var commonTraitCityId = int.Parse(responseCommonTraitCity["id"].ToString());
            var commonTraitDegreeId = int.Parse(responseCommonTraitDegree["id"].ToString());

            SendAdminRequest<JObject>($"/api/admin/entity-traits-course/{courseId}/{commonTraitCityId}", null,
                HttpMethod.Post);
            SendAdminRequest<JObject>($"/api/admin/entity-traits-course/{courseId}/{commonTraitDegreeId}", null,
                HttpMethod.Post);

            SendAdminRequest<JObject>($"/api/admin/entity-traits-school/{id}/{commonTraitCityId}", null,
                HttpMethod.Post);
            SendAdminRequest<JObject>($"/api/admin/entity-traits-school/{id}/{commonTraitDegreeId}", null,
                HttpMethod.Post);

            SendAdminRequest<JObject>($"/api/admin/entity-trait-types/{EntityType.School}/{commonTraitCityId}",
                null, HttpMethod.Post);
            SendAdminRequest<JObject>($"/api/admin/entity-trait-types/{EntityType.School}/{commonTraitDegreeId}",
                null, HttpMethod.Post);

            var commonTraitUniversityIdList =
                SendAdminRequest<List<int>>($"/api/admin/entity-traits-course/{courseId}", null);

            Assert.Contains(commonTraitCityId, commonTraitUniversityIdList);
            Assert.Contains(commonTraitDegreeId, commonTraitUniversityIdList);

            var catalogFilterResponse = SendAnonRequest<CatalogSearchResponseDtoList<CatalogCourseDto>>($"/api/catalog/course/search/ru", filter);

            Assert.NotNull(catalogFilterResponse.Items.FirstOrDefault(courseEntity => courseEntity.Name == course["languages"]["ru"]["name"].ToString()));
        }

        public static IEnumerable<object[]> Data()
        {
            var school = SchoolDataSet.GetSchool();
            var course = CourseDataSet.GetCourse(0);
            var commonTrait = TraitDataSet.GetCommonTrait();
            var cityTraitType = TraitDataSet.GetTraitType("city");
            var degreeTraitType = TraitDataSet.GetTraitType("degree");
            var filter = JObject.FromObject(new
            {
                pageNumber = 1,
                filters = new object[]
                {
                    new
                    {
                        identifier = "city",
                        values = new object []{1},
                    }
                }
            });

            return new List<object[]>
            {
                new object[]
                {
                    school,
                    1991,
                    course,
                    commonTrait,
                    cityTraitType,
                    degreeTraitType,
                    filter
                },
            };

        }
    }
}
