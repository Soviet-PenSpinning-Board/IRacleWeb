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
    [Route("api/changes")]
    [ApiController]
    public class ChangesApiController : ControllerBase
    {
        private readonly ILogger<ChangesApiController> _logger;
        private readonly ITierListContainerService _tierListContainer;
        private readonly IChangesContainerService _changesContainer;

        private readonly ITokenManager _tokenManager;

        public ChangesApiController(ILogger<ChangesApiController> logger, ITierListContainerService tierListContainer, IChangesContainerService changesContainer, ITokenManager tokenManager)
        {
            _logger = logger;
            _tierListContainer = tierListContainer;
            _changesContainer = changesContainer;
            _tokenManager = tokenManager;
        }

        /// <summary>
        /// Возвращает общее количество изменений.
        /// </summary>
        /// <returns>Количество изменений <see cref="int"/>.</returns>
        /// <response code="200">Количество изменений.</response>
        /// <response code="400">При прочих проблемах с запросом.</response>
        [HttpGet("count")]
        [ProducesResponseType<IEnumerable<ChangeBaseModel>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCount()
        {
            try
            {
                return Ok(await _changesContainer.GetCount());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка: ");
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Возвращает последние изменения исходя из параметров.
        /// </summary>
        /// <param name="offset">Оффсет от 0 для сдвига.</param>
        /// <param name="limit">Лимит возвращаемых объектов, не больше 100.</param>
        /// <returns><see cref="List{T}"/> из <see cref="ChangeBaseModel"/>, а конкретно его потомков.</returns>
        /// <response code="200">Возвращает изменения.</response>
        /// <response code="400">При прочих проблемах с запросом.</response>
        [HttpGet("")]
        [ProducesResponseType<IEnumerable<ChangeBaseModel>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetChanges(int offset = 0, int limit = 10)
        {
            try
            {
                return Ok(await _changesContainer.GetAllChanges(offset, Math.Max(limit, 100)));
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
        [HttpPost("add")]
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

        private bool CheckPermissions(string token, Permissions needPermissiosn)
        {
            Permissions targetPermissions = _tokenManager.CheckToken(token);
            return (needPermissiosn & targetPermissions) == needPermissiosn;
        }
    }
}
