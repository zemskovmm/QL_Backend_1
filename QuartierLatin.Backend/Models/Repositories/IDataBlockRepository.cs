using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories
{
    public interface IDataBlockRepository
    {
        public int CreateDataBlock(string title, string blockData, int languageId, int pageId);

        public Task<IList<DataBlock>> GetDataBlockListForPageAsync(int pageId);

        public Task<IList<DataBlock>> GetDataBlockListForPageAndLanguageAsync(int languageId, int pageId);

        public Task<int> RemoveDataBlockAsync(int dataBlockId);

        public Task EditDataBlockAsync(DataBlock dataBlock);
    }
}
