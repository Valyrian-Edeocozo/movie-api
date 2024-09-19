using System.Reflection;

namespace movie_api.ApiLayer.Infrastructure
{
    public static class WebApplicationExtensions
    {
        public static RouteGroupBuilder MapGroup(this WebApplication app, BaseEndpointGroup group)
        {
            var groupName = group.GetType().Name;

            return app
                .MapGroup($"/api/{groupName}")
                .WithGroupName(groupName)
                .WithTags(groupName)
                .WithOpenApi();
        }

        public static WebApplication MapEndpoints(this WebApplication app)
        {
            var endpointGroupType = typeof(BaseEndpointGroup);

            var assembly = Assembly.GetExecutingAssembly();

            var endpointGroupTypes = assembly.GetExportedTypes()
                .Where(t => t.IsSubclassOf(endpointGroupType));

            foreach (var type in endpointGroupTypes)
            {
                if (Activator.CreateInstance(type) is BaseEndpointGroup instance)
                {
                    instance.Map(app);
                }
            }

            return app;
        }
    }
}
