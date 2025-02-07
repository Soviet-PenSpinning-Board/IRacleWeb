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
    }
}
