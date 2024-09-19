namespace movie_api.ApiLayer.EndpointsCommon
{
    public interface IEndpointDefinition
    {
        void DefineServices(IServiceCollection services);
        void DefineEndPoints(WebApplication app);
    }
}
