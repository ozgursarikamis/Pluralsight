using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace LifeCycle.Filters
{
    public class OutageAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        private IConfiguration _config;
        public OutageAuthorizationFilter(IConfiguration config)
        {
            _config = config;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var appSwitch = _config.GetSection("FeatureSwitches")
            //    .GetChildren().FirstOrDefault(x => x.Key == "Outage");

            //if (bool.Parse(appSwitch.Value))
            //{
            //    context.Result = new ViewResult { ViewName = "Outage" };
            //}
        }
    }
}
