namespace movie_api.InfrastructureLayer.Interfaces
{
    public interface ICacheRepository<T>
    {
        bool AddToCache(T model, string cacheKey);
        T GetFromCache(string cacheKey);
    }
}
