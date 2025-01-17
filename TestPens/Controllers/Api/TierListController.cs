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
        private readonly IPersonContainerService _containerService;
        private readonly ITokenManager _tokenManager;

        public TierListController(ILogger<TierListController> logger, IPersonContainerService containerService, ITokenManager tokenManager)
        {
            _logger = logger;
            _containerService = containerService;
            _tokenManager = tokenManager;
        }

        [HttpGet("head")]
        public IActionResult GetHead()
        {
            try
            {
                TierListState head = _containerService.GetHead();
                return Ok(head.TierList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return Problem(ex.ToString());
            }
        }

        [HttpGet("changes")]
        public IActionResult GetChanges()
        {
            try
            {
                IReadOnlyCollection<BaseChange> changes = _containerService.GetAllChanges();
                return Ok(changes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return Problem(ex.ToString());
            }
        }

        [HttpPost("addchange")]
        public IActionResult AddChange(string token, [FromBody] BaseChange change)
        {
            Permissions neededPermissions = change.GetPermission();

            if (!CheckPermissions(token, neededPermissions))
            {
                return Unauthorized();
            }

            try
            {
                _containerService.AddChange(change);
                TierListState head = _containerService.GetHead();
                return Ok(head.TierList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return Problem(ex.ToString());
            }
        }

        [HttpPost("addchanges")]
        public IActionResult AddChanges(string token, [FromBody] List<BaseChange> changes)
        {
            Permissions neededPermissions = Permissions.None;

            foreach (BaseChange change in changes)
            {
                neededPermissions |= change.GetPermission();
            }

            if (!CheckPermissions(token, neededPermissions))
            {
                return Unauthorized();
            }

            try
            {
                foreach (BaseChange change in changes)
                {
                    _containerService.AddChange(change);
                }
                TierListState head = _containerService.GetHead();
                return Ok(head.TierList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return Problem(ex.ToString());
            }
        }

        [HttpPost("revertlast")]
        public IActionResult RevertLast(string token)
        {
            if (!CheckPermissions(token, Permissions.GlobalChanges))
            {
                return Unauthorized();
            }

            try
            {
                _containerService.RevertLast(1);
                TierListState head = _containerService.GetHead();
                return Ok(head.TierList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return Problem(ex.ToString());
            }
        }

        private bool CheckPermissions(string token, Permissions needPermissiosn)
        {
            Permissions targetPermissions = _tokenManager.CheckToken(token);
            return (needPermissiosn & targetPermissions) == needPermissiosn;
        }
    }
}
