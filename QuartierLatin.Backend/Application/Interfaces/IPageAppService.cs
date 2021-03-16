using System.Threading.Tasks;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Dto.AdminPageModuleDto;
using QuartierLatin.Backend.Dto.PageModuleDto;

namespace QuartierLatin.Backend.Application.Interfaces
{
    public interface IPageAppService
    {
        public Task<RouteDto<PageModuleDto>> GetPageByUrlAsync(string url);

        public Task<RouteDto<AdminPageModuleDto>> GetPageByUrlAdminAsync(string url);

        public int CreatePage(CreatePageDto createPageDto);
    }
}
