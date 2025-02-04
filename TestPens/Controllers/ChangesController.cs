using System.Diagnostics;
using System.IO.Pipes;

using Microsoft.AspNetCore.Mvc;

using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Controllers
{
    public class ChangesController : Controller
    {
        private readonly ILogger<MainController> _logger;

        public ChangesController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
