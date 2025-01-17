using Microsoft.AspNetCore.Mvc;

using TestPens.Service.Abstractions;

namespace TestPens.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly IPersonContainerService containerService;

        public AdminController(ILogger<MainController> logger, IPersonContainerService containerService)
        {
            _logger = logger;
            this.containerService = containerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckPassword(string password)
        {
            // TODO: нормальная система токенов и доступов
            if (password == "secret")
            {
                return PartialView("_UnlockedContent", containerService.GetHead().TierList);
            }
            else
            {
                return Content("Неверное слово. Самый умный нашелся?");
            }
        }
    }
}
