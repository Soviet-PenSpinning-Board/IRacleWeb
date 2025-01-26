using Microsoft.AspNetCore.Mvc;

using TestPens.Models;
using TestPens.Models.Abstractions;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Controllers.Api
{
    [Route("api/battles")]
    [ApiController]
    public class BattlesApiController : ControllerBase
    {
        private readonly ILogger<BattlesApiController> _logger;
        private readonly IBattleControllerService _battleService;
        private readonly ITokenManager _tokenManager;

        public BattlesApiController(ILogger<BattlesApiController> logger, IBattleControllerService battleService, ITokenManager tokenManager)
        {
            _logger = logger;
            _battleService = battleService;
            _tokenManager = tokenManager;
        }

        [HttpGet("getunactive")]
        public IActionResult GetAllBattles(int offset = 0, int limit = 100)
        {
            try
            {
                return Ok(_battleService.GetUnactiveBattles(offset, limit));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return Problem(ex.ToString());
            }
        }

        [HttpGet("getactive")]
        public IActionResult GetActiveBattles()
        {
            try
            {
                return Ok(_battleService.GetActiveBattles());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return Problem(ex.ToString());
            }
        }

        [HttpPost("add")]
        public IActionResult CreateBattle(string token, [FromBody] BattleModel battle)
        {
            if (!CheckPermissions(token, Permissions.StartBattles))
            {
                return Unauthorized();
            }

            try
            {
                return Ok(_battleService.AddBattle(battle));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return Problem(ex.ToString());
            }
        }

        [HttpPut("update")]
        public IActionResult UpdateResult(string token, Guid guid, BattleResult result, bool positionChange = true)
        {
            if (!CheckPermissions(token, Permissions.EndBattles))
            {
                return Unauthorized();
            }

            try
            {
                if (_battleService.ChangeResult(guid, result, positionChange))
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
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
