using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace LifeCycle.Middleware
{
    public class FeatureSwitchAuthMiddleware
    {
        private readonly RequestDelegate _next;
        public FeatureSwitchAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IConfiguration config)
        {
            var endpoint = httpContext.GetEndpoint()?.Metadata.GetMetadata<RouteAttribute>();
           
            if (endpoint != null)
            {
                var featureSwitch = config.GetSection("FeatureSwitch")
                    .GetChildren().FirstOrDefault(x => x.Key == endpoint.Name);

                if (featureSwitch != null && !bool.Parse(featureSwitch.Value))
                {
                    httpContext.SetEndpoint(new Endpoint(context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        return Task.CompletedTask;
                    },
                    EndpointMetadataCollection.Empty, "Feature Not Found"
                    ));
                }

                await _next(httpContext);
            }
        }
    }
}
