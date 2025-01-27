using System.Diagnostics;
using System.Globalization;
using System.IO.Pipes;

using Microsoft.AspNetCore.Mvc;

using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Controllers
{
    public class TierListController : Controller
    {
        private readonly ILogger<TierListController> _logger;
        private readonly IPersonContainerService containerService;

        public TierListController(ILogger<TierListController> logger, IPersonContainerService containerService)
        {
            _logger = logger;
            this.containerService = containerService;
        }

        public IActionResult Index()
        {
            return View(containerService.GetHead().TierList);
        }

        public IActionResult TimeMachine()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TimeMachineMain(DateTime dateTime)
        {
            return PartialView("_MainTierListPartial", containerService.RevertAllAfterNode(dateTime).TierList);
        }
    }
}
