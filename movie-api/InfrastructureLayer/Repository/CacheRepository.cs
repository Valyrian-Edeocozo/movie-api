using Microsoft.Extensions.Caching.Memory;
using movie_api.InfrastructureLayer.Interfaces;
using System.Reflection;

namespace movie_api.InfrastructureLayer.Repository
{
    public class CacheRepository<T> : ICacheRepository<T> where T : class
    {
        private readonly IMemoryCache memoryCache;
        private readonly IConfiguration _configuration;

        public CacheRepository(IMemoryCache memoryCache, IConfiguration configuration)
        {
            this.memoryCache = memoryCache;
            this._configuration = configuration;
        }
        public bool AddToCache(T model, string cacheKey)
        {
            var modelValue = memoryCache.Set(cacheKey, model, TimeSpan.FromMinutes(int.Parse(_configuration["TimeConfig:CacheTime"]!)));

            if(modelValue == null)
            {
                return false;
            }
            return true;
        }

        public T GetFromCache(string cacheKey)
        {

            if (memoryCache.TryGetValue(cacheKey, out T? result))
            {
                return result!;
            }

            return default!;
        }
    }
}
