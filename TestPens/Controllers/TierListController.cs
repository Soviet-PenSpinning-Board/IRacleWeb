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
        private readonly ITierListContainerService _tierListContainer;
        private readonly IChangesContainerService _changesContainer;

        public TierListController(ILogger<TierListController> logger, ITierListContainerService tierListContainer, IChangesContainerService changesContainer)
        {
            _logger = logger;
            _tierListContainer = tierListContainer;
            _changesContainer = changesContainer;
        }

        public IActionResult Index(TierListType type = TierListType.Main)
        {
            return View((type.GetView(), _tierListContainer.GetHead().TierList));
        }

        public IActionResult TimeMachine()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TimeMachineMain(DateTime dateTime, TierListType type = TierListType.Main)
        {
            return PartialView("TierList/_FullTierListPartial", (type.GetView(), (await _changesContainer.RevertAllAfterNode(dateTime)).TierList));
        }
    }
}
