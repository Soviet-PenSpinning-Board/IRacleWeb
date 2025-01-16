using System.Diagnostics;
using System.IO.Pipes;

using Microsoft.AspNetCore.Mvc;

using TestPens.Models;

namespace TestPens.Controllers
{
    public class BattleController : Controller
    {
        private readonly ILogger<MainController> _logger;

        public BattleController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
