using System.Diagnostics;
using System.IO.Pipes;

using Microsoft.AspNetCore.Mvc;

using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Controllers
{
    public class MainController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly IPersonContainerService containerService;

        public MainController(ILogger<MainController> logger, IPersonContainerService containerService)
        {
            _logger = logger;
            this.containerService = containerService;
        }

        public IActionResult Index()
        {
            return View(containerService.GetHead().TierList);
        }
    }
}
