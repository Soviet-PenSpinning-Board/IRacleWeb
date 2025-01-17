using Microsoft.AspNetCore.Mvc;

using TestPens.Models;
using TestPens.Models.Abstractions;
using TestPens.Service.Abstractions;

namespace TestPens.Controllers.Api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TierListController : ControllerBase
    {
        private readonly ILogger<TierListController> _logger;
        private readonly IPersonContainerService containerService;

        public TierListController(ILogger<TierListController> logger, IPersonContainerService containerService)
        {
            _logger = logger;
            this.containerService = containerService;
        }

        [HttpGet("head")]
        public IActionResult GetHead()
        {
            try
            {
                TierListState head = containerService.GetHead();
                return Ok(head.TierList);
            }
            catch (Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        [HttpGet("changes")]
        public IActionResult GetChanges()
        {
            try
            {
                IReadOnlyCollection<BaseChange> changes = containerService.GetAllChanges();
                return Ok(changes);
            }
            catch (Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        [HttpPost("addchange")]
        public IActionResult AddChange(string token, [FromBody] BaseChange change)
        {
            try
            {
                containerService.AddChange(change);
                TierListState head = containerService.GetHead();
                return Ok(head.TierList);
            }
            catch (Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        [HttpPost("addchanges")]
        public IActionResult AddChanges(string token, [FromBody] List<BaseChange> changes)
        {
            try
            {
                foreach (BaseChange change in changes)
                {
                    containerService.AddChange(change);
                }
                TierListState head = containerService.GetHead();
                return Ok(head.TierList);
            }
            catch (Exception ex)
            {
                return Problem(ex.ToString());
            }
        }
    }
}
