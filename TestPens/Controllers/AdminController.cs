using Microsoft.AspNetCore.Mvc;

using TestPens.Service;
using TestPens.Service.Abstractions;

namespace TestPens.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ITierListContainerService _personService;
        private readonly IBattleControllerService _battleService;

        private readonly ITokenManager _tokenManager;

        public AdminController(ILogger<AdminController> logger, ITierListContainerService containerService, ITokenManager tokenManager, IBattleControllerService battleService)
        {
            _logger = logger;
            _personService = containerService;
            _tokenManager = tokenManager;
            _battleService = battleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckPassword(string password)
        {
            Permissions permissions = _tokenManager.CheckToken(password);
            if (permissions != Permissions.None)
            {
                return PartialView("_UnlockedContent", (permissions, _personService.GetHead().TierList, await _battleService.GetActiveBattles()));
            }
            else
            {
                return Content("Неверный токен. Самый умный нашелся?");
            }
        }
    }
}
