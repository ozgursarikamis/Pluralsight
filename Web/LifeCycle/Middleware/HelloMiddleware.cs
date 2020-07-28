using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LifeCycle.Middleware
{
    public class HelloMiddleware
    {
        private RequestDelegate _next;
        public HelloMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Logic Here
            await _next.Invoke(context);
        }
    }
}
