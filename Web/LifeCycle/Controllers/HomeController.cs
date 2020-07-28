using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LifeCycle.Models;
using LifeCycle.Filters;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace LifeCycle.Controllers
{
    [TypeFilter(typeof(OutageAuthorizationFilter))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        [ModelBinder]
        public string Id { get; set; }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
         
        [Route("/index", Name = "Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, Route("/contact-us", Name = "Contact")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost, Route("/contact-us", Name = "Contact")]
        public IActionResult Privacy([FromForm]Contact info)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    public class Contact
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }
    public class BindedContact: IModelBinder
    {
        public Guid Id { get; set; }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            throw new NotImplementedException();
        }
    }
}
