using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.Repositories;

namespace QuartierLatin.Backend.Database.Repositories
{
    public class SqlDataBlockRepository : IDataBlockRepository
    {
        private readonly AppDbContextManager _db;

        public SqlDataBlockRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public int CreateDataBlock(string type, string blockData, int languageId, int pageId, int blockRootId = 0)
        {
            var block = new DataBlock
            {
                BlockData = blockData,
                LanguageId = languageId,
                PageId = pageId,
                BlockRootId = 0,
                Type = type
            };

            var rootId =  _db.Exec(db => db.InsertWithInt32Identity(block));

            if(blockRootId is 0)
            {
                block.BlockRootId = rootId;
                block.Id = rootId;
            }

            _db.Exec(db => db.Update(block));
            return rootId;
        }

        public async Task EditDataBlockAsync(DataBlock dataBlock)
        {
            await _db.ExecAsync(db => db
                .UpdateAsync(dataBlock));
        }

        public async Task<IList<DataBlock>> GetDataBlockListForPageAsync(int pageId)
        {
            return await _db.ExecAsync(db => db.DataBlocks.Where(block => block.PageId == pageId).ToListAsync());
        }

        public async Task<IList<DataBlock>> GetDataBlockListForPageAndLanguageAsync(int languageId, int pageId)
        {
            return await _db.ExecAsync(db => db.DataBlocks.Where(block => block.PageId == pageId && block.LanguageId == languageId).ToListAsync());
        }

        public async Task<int> RemoveDataBlockAsync(int dataBlockId)
        {
            return await _db.ExecAsync(db => db.DataBlocks.Where(block => block.Id == dataBlockId).DeleteAsync());
        }

        public async Task<IList<DataBlock>> GetDataBlockListForPageByBlockRootIdAsync(int blockRootId)
        {
            return await _db.ExecAsync(db => db.DataBlocks.Where(block => block.BlockRootId == blockRootId).ToListAsync());
        }
    }
}
