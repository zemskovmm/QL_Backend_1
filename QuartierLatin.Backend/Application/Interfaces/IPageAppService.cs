using System.Threading.Tasks;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Dto.PageModuleDto;

namespace QuartierLatin.Backend.Application.Interfaces
{
    public interface IPageAppService
    {
        public Task<RouteDto<PageModuleDto>> GetPageByUrl(string url);
    }
}
