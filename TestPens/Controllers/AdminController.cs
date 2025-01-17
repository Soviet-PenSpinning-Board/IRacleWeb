using Microsoft.AspNetCore.Mvc;

using TestPens.Service;
using TestPens.Service.Abstractions;

namespace TestPens.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly IPersonContainerService _containerService;
        private readonly ITokenManager _tokenManager;

        public AdminController(ILogger<MainController> logger, IPersonContainerService containerService, ITokenManager tokenManager)
        {
            _logger = logger;
            _containerService = containerService;
            _tokenManager = tokenManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckPassword(string password)
        {
            Permissions permissions = _tokenManager.CheckToken(password);
            if (permissions != Permissions.None)
            {
                return PartialView("_UnlockedContent", (permissions, _containerService.GetHead().TierList, password));
            }
            else
            {
                return Content("Неверный токен. Самый умный нашелся?");
            }
        }
    }
}
