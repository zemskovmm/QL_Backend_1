using Newtonsoft.Json;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Dto.AdminPageModuleDto;
using QuartierLatin.Backend.Models.Repositories;

namespace QuartierLatin.Backend.Application
{
    public class DataBlockAppService : IDataBlockAppService
    {
        private readonly IDataBlockRepository _dataBlockRepository;

        public DataBlockAppService(IDataBlockRepository dataBlockRepository)
        {
            _dataBlockRepository = dataBlockRepository;
        }

        public int CreateDataBlockForPage(CreateDataBlockDto createDataBlockDto)
        {
            return _dataBlockRepository.CreateDataBlock(createDataBlockDto.Type, JsonConvert.SerializeObject(createDataBlockDto.BlockData),
                createDataBlockDto.LanguageId,
                createDataBlockDto.PageId,
                createDataBlockDto.BlockRootId);
        }
    }
}
