using Microsoft.AspNetCore.Mvc;

using TestPens.Models;
using TestPens.Models.Dto;
using TestPens.Models.Real;
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

        /// <summary>
        /// Возвращает коллекцию битв исходя из параметров.
        /// </summary>
        /// <param name="offset">Оффсет от 0 для сдвига.</param>
        /// <param name="limit">Лимит возвращаемых объектов.</param>
        /// <returns>Словарь, ключ - <see cref="Guid"/> значение - <see cref="BattleDatabase"/>.</returns>
        /// <response code="200">Возвращает коллекцию битв.</response>
        /// <response code="400">При прочих проблемах с запросом.</response>
        [HttpGet("getunactive")]
        [ProducesResponseType<IReadOnlyDictionary<Guid, BattleDatabase>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllBattles(int offset = 0, int limit = 100)
        {
            try
            {
                return Ok(await _battleService.GetUnactiveBattles(offset, limit));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Возвращает коллекцию только активных битв.
        /// </summary>
        /// <returns>Словарь, ключ - <see cref="Guid"/> значение - <see cref="BattleDatabase"/>.</returns>
        /// <response code="200">Возвращает коллекцию активных битв.</response>
        /// <response code="400">При прочих проблемах с запросом.</response>
        [HttpGet("getactive")]
        [ProducesResponseType<IReadOnlyDictionary<Guid, BattleDatabase>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetActiveBattles()
        {
            try
            {
                return Ok(await _battleService.GetActiveBattles());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Создает битву, из <see cref="BattleDto"/>.
        /// Требует авторизации.
        /// </summary>
        /// <param name="token">Токен авторизации.</param>
        /// <param name="battle">Объект <see cref="BattleDto"/> для создания битвы.</param>
        /// <returns><see cref="Guid"/> созданной битвы.</returns>
        /// <response code="200">Возвращает <see cref="Guid"/> созданной битвы.</response>
        /// <response code="400">При прочих проблемах с запросом.</response>
        /// <response code="401">При недостатке прав токена.</response>
        [HttpPost("add")]
        [ProducesResponseType<Guid>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBattle(string token, [FromBody] BattleDto battle)
        {
            if (!CheckPermissions(token, Permissions.StartBattles))
            {
                return Unauthorized();
            }

            try
            {
                return Ok(await _battleService.AddBattle(battle));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Обновляет результат битв.
        /// Требует авторизации.
        /// </summary>
        /// <param name="token">Токен авторизации.</param>
        /// <param name="guid"><see cref="Guid"/> битвы который надо поменять.</param>
        /// <param name="result">новый <see cref="BattleResult"/> битвы.</param>
        /// <param name="positionChange">Нужно ли менять позицию людей при каких-либо возможных изменениях.</param>
        /// <param name="updateWinnerVideo">Нужно ли обновлять закрепленную комбу у победителя.</param>
        /// <returns>Пустой статус код.</returns>
        /// <response code="200">Результат поменялся.</response>
        /// <response code="404">Битва не найдена.</response>
        /// <response code="400">При прочих проблемах с запросом.</response>
        /// <response code="401">При недостатке прав токена.</response>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateResult(string token, Guid guid, BattleResult result, bool positionChange = true, bool updateWinnerVideo = false)
        {
            if (!CheckPermissions(token, Permissions.EndBattles))
            {
                return Unauthorized();
            }

            try
            {
                if (await _battleService.ChangeResult(guid, result, positionChange, updateWinnerVideo))
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
                return BadRequest(ex.ToString());
            }
        }

        private bool CheckPermissions(string token, Permissions needPermissiosn)
        {
            Permissions targetPermissions = _tokenManager.CheckToken(token);
            return (needPermissiosn & targetPermissions) == needPermissiosn;
        }
    }
}
