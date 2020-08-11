using Microsoft.Extensions.DependencyInjection;
using TennisBookings.Web.Configuration;

namespace TennisBookings.Web.Core.DependencyInjection
{
    public static class ContentServiceCollectionExtensions
    {
        public static IServiceCollection AddContentServices(this IServiceCollection services)
        {
            services.AddSingleton<IProfanityChecker, ProfanityChecker>();
            return services;
        }
    }
}
