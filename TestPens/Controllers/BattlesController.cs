using System.Diagnostics;
using System.IO.Pipes;

using Microsoft.AspNetCore.Mvc;

using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Controllers
{
    public class BattlesController : Controller
    {
        private readonly ILogger<BattlesController> _logger;
        private readonly IBattleControllerService battleService;

        public BattlesController(ILogger<BattlesController> logger, IBattleControllerService battleService)
        {
            _logger = logger;
            this.battleService = battleService;
        }

        public IActionResult Index()
        {
            return View(battleService.GetActiveBattles());
        }
    }
}
