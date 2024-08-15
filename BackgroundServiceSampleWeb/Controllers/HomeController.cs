using BackgroundServiceSampleWeb.Models;
using BackgroundServiceSampleWeb.Queue;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BackgroundServiceSampleWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceProvider _serviceProvider;
        public HomeController(ILogger<HomeController> logger, SamplePruducer samplePruducer, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public string SampleAction()
        {
            SamplePruducer samplePruducer = _serviceProvider.GetRequiredService<SamplePruducer>();
            samplePruducer.SampleMethodBackground();
            return "در حال پردازش";
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}