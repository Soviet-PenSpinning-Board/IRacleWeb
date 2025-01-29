using Microsoft.AspNetCore.Mvc;

using TestPens.Models;
using TestPens.Models.Dto;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Real.Changes;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Controllers.Api
{
    [Route("api/TierList")]
    [ApiController]
    public class TierListApiController : ControllerBase
    {
        private readonly ILogger<TierListApiController> _logger;
        private readonly IPersonContainerService _containerService;
        private readonly ITokenManager _tokenManager;

        public TierListApiController(ILogger<TierListApiController> logger, IPersonContainerService containerService, ITokenManager tokenManager)
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
        public IActionResult GetChanges(int offset = 0, int limit = 100, DateTime? after = null)
        {
            try
            {
                IEnumerable<ChangeBaseModel> changes = _containerService.GetAllChanges(offset, limit, after);
                return Ok(changes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return Problem(ex.ToString());
            }
        }

        [HttpPost("addchanges")]
        public IActionResult AddChanges(string token, [FromBody] List<ChangeBaseDto> changes)
        {
            Permissions neededPermissions = Permissions.None;

            TierListState head = _containerService.GetHead();
            foreach (ChangeBaseDto change in changes)
            {
                neededPermissions |= change.Type.GetPermissions();
            }

            if (!CheckPermissions(token, neededPermissions))
            {
                return Unauthorized();
            }

            try
            {
                _containerService.AddChanges(changes);

                return Ok(_containerService.GetHead().TierList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return Problem(ex.ToString());
            }
        }

        [HttpPost("revert/{count}")]
        public IActionResult RevertLast(int count, string token)
        {
            if (!CheckPermissions(token, Permissions.GlobalChanges))
            {
                return Unauthorized();
            }

            try
            {
                _containerService.RevertLast(count);
                TierListState head = _containerService.GetHead();
                return Ok(head.TierList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return Problem(ex.ToString());
            }
        }

        [HttpGet("node/reverttime")]
        public IActionResult RevertAfterNode(DateTime dateTime)
        {
            try
            {
                return Ok(_containerService.RevertAllAfterNode(dateTime).TierList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return Problem(ex.ToString());
            }
        }

        [HttpGet("node/revert/{count}")]
        public IActionResult RevertAfterNode(int count)
        {
            try
            {
                return Ok(_containerService.RevertLastNode(count).TierList);
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
