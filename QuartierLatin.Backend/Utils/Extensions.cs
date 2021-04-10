using System;
using System.Threading.Tasks;
using LinqToDB.Data;

namespace QuartierLatin.Backend.Utils
{
    public static class Extensions
    {
        public async static Task<T> InTransaction<T>(this DataConnection conn, Func<Task<T>> cb)
        {
            await using (var t = await conn.BeginTransactionAsync())
            {
                var rv = await cb();
                await t.CommitAsync();
                return rv;
            }
        }
    }
}