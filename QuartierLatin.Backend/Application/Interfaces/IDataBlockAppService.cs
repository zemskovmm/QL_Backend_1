using System.Threading.Tasks;
using QuartierLatin.Backend.Dto.AdminPageModuleDto;

namespace QuartierLatin.Backend.Application.Interfaces
{
    public interface IDataBlockAppService
    {
        public int CreateDataBlockForPage(CreateDataBlockDto createDataBlockDto);
    }
}
