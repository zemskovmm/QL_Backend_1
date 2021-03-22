using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Storages.Cache
{
    public class GlobalSettingsCache<TItem>
    {
        private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions()
        {
            SizeLimit = 8124,
        });

        private ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

        public async Task<TItem> GetOrCreateAsync(object key, Func<Task<TItem>> createItem)
        {
            if (_cache.TryGetValue(key, out TItem cacheEntry)) return cacheEntry;

            var myLock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            await myLock.WaitAsync();
            try
            {
                if (!_cache.TryGetValue(key, out cacheEntry))
                {
                    cacheEntry = await createItem();
                    _cache.Set(key, cacheEntry, GetMemoryCacheEntryOptions());
                }
            }
            finally
            {
                myLock.Release();
            }
            return cacheEntry;
        }

        public async Task UpdateDataInCacheAsync(object key, Func<Task<TItem>> createItem)
        {
            var myLock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            await myLock.WaitAsync();
            try
            {
                var cacheEntry = await createItem();
                _cache.Set(key, cacheEntry, GetMemoryCacheEntryOptions());
            }
            finally
            {
                myLock.Release();
            }
            return;
        }

        public async Task RemoveDataInCacheAsync(object key)
        {
            var myLock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            await myLock.WaitAsync();
            try
            {
                _cache.Remove(key);
            }
            finally
            {
                myLock.Release();
            }
            return;
        }

        public async Task<object> GetCachedDataAsync(object key)
        {
            return _cache.TryGetValue(key, out TItem cacheEntry) ? (object) cacheEntry : null;
        }

        private MemoryCacheEntryOptions GetMemoryCacheEntryOptions() => new MemoryCacheEntryOptions()
            .SetSize(64)
            .SetPriority(CacheItemPriority.High)
            .SetSlidingExpiration(TimeSpan.FromSeconds(3600))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(7200));
    }
}
