using Microsoft.AspNetCore.Mvc;

using TestPens.Models;
using TestPens.Models.Dto;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Real;
using TestPens.Models.Real.Changes;
using TestPens.Models.Shared;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Controllers.Api
{
    [Route("api/TierList")]
    [ApiController]
    public class TierListApiController : ControllerBase
    {
        private readonly ILogger<TierListApiController> _logger;
        private readonly ITierListContainerService _tierListContainer;
        private readonly IChangesContainerService _changesContainer;

        private readonly ITokenManager _tokenManager;

        public TierListApiController(ILogger<TierListApiController> logger, ITierListContainerService tierListContainer, IChangesContainerService changesContainer, ITokenManager tokenManager)
        {
            _logger = logger;
            _tierListContainer = tierListContainer;
            _changesContainer = changesContainer;
            _tokenManager = tokenManager;
        }

        /// <summary>
        /// Возвращает текущее состояние тирлиста.
        /// </summary>
        /// <returns>Словарь, ключ - <see cref="Tier"/> значение - <see cref="List{T}"/> из <see cref="PersonModel"/>.</returns>
        /// <response code="200">Возвращает тирлист.</response>
        /// <response code="400">При прочих проблемах с запросом.</response>
        [HttpGet("head")]
        [ProducesResponseType<IReadOnlyDictionary<Tier, List<PersonModel>>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetHead()
        {
            try
            {
                TierListState head = _tierListContainer.GetHead();
                return Ok(head.TierList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Возвращает все изменения исходя из параметров.
        /// </summary>
        /// <param name="offset">Оффсет от 0 для сдвига.</param>
        /// <param name="limit">Лимит возвращаемых объектов.</param>
        /// <param name="after">Время, начиная с которого будут возвращаться изменения. В ФОРМАТЕ UTC</param>
        /// <returns><see cref="List{T}"/> из <see cref="ChangeBaseModel"/>, а конкретно его потомков.</returns>
        /// <response code="200">Возвращает изменения.</response>
        /// <response code="400">При прочих проблемах с запросом.</response>
        [HttpGet("changes")]
        [ProducesResponseType<IEnumerable<ChangeBaseModel>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetChanges(int offset = 0, int limit = 100, DateTime? after = null)
        {
            try
            {
                return Ok(await _changesContainer.GetAllChanges(offset, limit, after));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Добавляет изменения в тирлист, используя список из <see cref="ChangeBaseDto"/> объектов, точнее их потомков.
        /// Требует авторизации.
        /// </summary>
        /// <param name="token">Токен авторизации.</param>
        /// <param name="changes">Список из <see cref="ChangeBaseDto"/>.</param>
        /// <returns>Новое состояние листа, после изменений.</returns>
        /// <response code="200">Новое состояние листа, после изменений.</response>
        /// <response code="400">При прочих проблемах с запросом.</response>
        /// <response code="401">При недостатке прав токена.</response>
        [HttpPost("addchanges")]
        [ProducesResponseType<IReadOnlyDictionary<Tier, List<PersonModel>>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddChanges(string token, [FromBody] List<ChangeBaseDto> changes)
        {
            Permissions neededPermissions = Permissions.None;

            TierListState head = _tierListContainer.GetHead();
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
                await _changesContainer.AddChanges(changes);

                return Ok(_tierListContainer.GetHead().TierList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Отменяет последние count именений с конца, ПРИМЕНЯЕТ ИЗМЕНЕНИЯ.
        /// Требует авторизации.
        /// </summary>
        /// <param name="token">Токен авторизации.</param>
        /// <param name="count">Сколько изменений с конца нужно отменить.</param>
        /// <returns>Новое состояние листа, после изменений.</returns>
        /// <response code="200">Новое состояние листа, после изменений.</response>
        /// <response code="400">При прочих проблемах с запросом.</response>
        /// <response code="401">При недостатке прав токена.</response>
        [HttpPost("revert/{count}")]
        [ProducesResponseType<IReadOnlyDictionary<Tier, List<PersonModel>>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RevertLast(int count, string token)
        {
            if (!CheckPermissions(token, Permissions.GlobalChanges))
            {
                return Unauthorized();
            }

            try
            {
                await _changesContainer.RevertLast(count);
                TierListState head = _tierListContainer.GetHead();
                return Ok(head.TierList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Локально отменяет последние count именений с конца.
        /// </summary>
        /// <param name="count">Сколько изменений с конца нужно отменить.</param>
        /// <returns>Состояние листа, после изменений.</returns>
        /// <response code="200">Новое состояние листа, после изменений.</response>
        /// <response code="400">При прочих проблемах с запросом.</response>
        [HttpGet("node/revert/{count}")]
        [ProducesResponseType<IReadOnlyDictionary<Tier, List<PersonModel>>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RevertLastNode(int count)
        {
            try
            {
                return Ok((await _changesContainer.RevertLastNode(count)).TierList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Локально отменяет все изменения после указаной даты.
        /// </summary>
        /// <param name="dateTime">Время, начиная с которого будут отменяться изменения. В ФОРМАТЕ UTC</param>
        /// <returns>Состояние листа, после изменений.</returns>
        /// <response code="200">Новое состояние листа, после изменений.</response>
        /// <response code="400">При прочих проблемах с запросом.</response>
        [HttpGet("node/reverttime")]
        [ProducesResponseType<IReadOnlyDictionary<Tier, List<PersonModel>>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RevertAfterNode(DateTime dateTime)
        {
            try
            {
                return Ok((await _changesContainer.RevertAllAfterNode(dateTime)).TierList);
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
