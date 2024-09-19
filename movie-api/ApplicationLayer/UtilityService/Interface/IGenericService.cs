namespace movie_api.ApplicationLayer.UtilityService.Interface
{
    public interface IGenericService
    {
        Task<string> ConsumeGetApi(string apiEndPoint, Dictionary<string, string> headers);
    }
}
