using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace StocksApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("/error")]
        public IActionResult Error()
        {
            var exceptions = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptions != null)
            {
                if (exceptions.Error != null)
                {
                    ViewBag.ErrorMessage = exceptions.Error.Message;
                }
            }
            return View();
        }
    }
}
