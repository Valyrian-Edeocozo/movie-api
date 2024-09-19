using movie_api.ApiLayer.Endponts;

namespace movie_api.ApiLayer.EndpointsCommon
{
    public class ServiceDefinition : IEndpointDefinition
    {
        public void DefineEndPoints(WebApplication app)
        {

            app.MapGroup("/movieservice")
                    .MovieFeedsGroup()
                    .RequireCors("corsapp")
                    .RequireRateLimiting("LimitPolicy")
                    .WithTags("MovieService");

        }
        public void DefineServices(IServiceCollection services)
        {

        }
    }
}
