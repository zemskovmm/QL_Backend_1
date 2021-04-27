using System;
using System.Collections.Generic;
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

        public static string GetSuitableName(this Dictionary<string, string> dic, string lang)
        {
            if (dic.TryGetValue(lang, out var rv))
                return rv;
            foreach(var l in new []{"en","fr","ru", "esp"})
                if (dic.TryGetValue(l, out rv))
                    return rv;
            return "<>";
        }
    }
}