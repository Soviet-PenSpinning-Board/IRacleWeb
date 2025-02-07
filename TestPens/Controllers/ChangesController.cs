using System.Diagnostics;
using System.IO.Pipes;

using Microsoft.AspNetCore.Mvc;

using TestPens.Extensions;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Controllers
{
    public class ChangesController : Controller
    {
        private readonly ILogger<ChangesController> _logger;
        private readonly ITierListContainerService _tierListContainer;
        private readonly IChangesContainerService _changesContainer;

        private const int PageSize = 12; 

        public ChangesController(ILogger<ChangesController> logger, ITierListContainerService tierListContainer, IChangesContainerService changesContainer)
        {
            _logger = logger;
            _tierListContainer = tierListContainer;
            _changesContainer = changesContainer;
        }

        public async Task<IActionResult> Index(int page = 0)
        {
            int count = await _changesContainer.GetCount();
            PaginatedInfo info = new PaginatedInfo(count, page, PageSize);
            return View((await _changesContainer.GetAllChanges(page * PageSize, PageSize), info));
        }
    }
}
