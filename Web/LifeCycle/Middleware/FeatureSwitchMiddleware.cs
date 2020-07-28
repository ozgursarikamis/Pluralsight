using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace LifeCycle.Middleware
{
    public class FeatureSwitchMiddleware
    {
        private readonly RequestDelegate _next;
        public FeatureSwitchMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IConfiguration config)
        {
            if (context.Request.Path.Value.Contains("/features"))
            {
                var switches = config.GetSection("FeatureSwitches");
                var report = switches.GetChildren().Select(x => $"{x.Key}: {x.Value}");
                await context.Response.WriteAsync(string.Join("\n", report));
            } else
            {
                await _next(context);
            }
        }
    }
}
