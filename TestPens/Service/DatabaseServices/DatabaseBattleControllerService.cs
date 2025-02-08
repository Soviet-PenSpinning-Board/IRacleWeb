using System.Text.Json;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Dto;
using TestPens.Models.Real;
using TestPens.Models.Shared;
using TestPens.Models.Simple;
using TestPens.Models;
using TestPens.Service.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TestPens.Models.Real.Changes;

namespace TestPens.Service.DatabaseServices;

public class DatabaseBattleControllerService : IBattleControllerService
{
    private readonly ILogger<DatabaseBattleControllerService> _logger;
    private readonly ApplicationContext _applicationContext;
    private readonly ITierListContainerService _tierListContainer;
    private readonly IChangesContainerService _changesContainer;

    public DatabaseBattleControllerService(ILogger<DatabaseBattleControllerService> logger, ApplicationContext applicationContext, ITierListContainerService tierListContainer, IChangesContainerService changesContainer)
    {
        _logger = logger;
        _applicationContext = applicationContext;
        _tierListContainer = tierListContainer;
        _changesContainer = changesContainer;
    }

    public async Task<IEnumerable<BattleDatabase>> GetUnactiveBattles(int offset = 0, int limit = int.MaxValue)
    {
        return await GetPredicate(battle => battle.Result != BattleResult.Unfinished).Skip(offset).Take(limit).ToListAsync();
    }

    public async Task<IEnumerable<BattleDatabase>> GetActiveBattles()
    {
        return await GetPredicate(battle => battle.Result == BattleResult.Unfinished).ToListAsync();
    }

    private IQueryable<BattleDatabase> GetPredicate(Expression<Func<BattleDatabase, bool>> predicate)
    {
        return _applicationContext.Battles.AsNoTracking().AsSplitQuery()!.OrderByDescending(b => b.UtcTime).Where(predicate);
    }

    public async Task<bool> ChangeResult(Guid guid, BattleResult battleResult, bool performPositionChange, bool updateWinnerCombo)
    {
        BattleDatabase? battle = await _applicationContext.Battles.FindAsync(guid);

        if (battle is null)
        {
            _logger.LogError("Битва: {guid} не найдена, метод {name}", guid, nameof(ChangeResult));
            return false;
        }

        battle.Result = battleResult;

        if (battleResult is BattleResult.RightWin)
        {
            if (performPositionChange)
            {
                TryChangeBattlePositions(battle.Right, battle.Left);
            }
            if (updateWinnerCombo)
            {
                UpdateWinnerVideo(battle.Right);
            }
        }
        if (battleResult is BattleResult.LeftWin)
        {
            if (performPositionChange)
            {
                TryChangeBattlePositions(battle.Left, battle.Right);
            }
            if (updateWinnerCombo)
            {
                UpdateWinnerVideo(battle.Left);
            }
        }

        await _applicationContext.SaveChangesAsync();

        return true;
    }

    private void TryChangeBattlePositions(BattledPersonModel winner, BattledPersonModel loser)
    {
        TierListState head = _tierListContainer.GetHead();
        (_, PositionModel winnerPos) = winner.GetActual(head);
        (_, PositionModel loserPos) = loser.GetActual(head);

        if (winnerPos == PositionModel.Unknown || loserPos == PositionModel.Unknown)
        {
            _logger.LogError("Один из участников баттла между {nick1} ({guid1}) и {nick2} ({guid2}) имеет неопределенную позицию", winner.MainModel!.Nickname, winner.MainModel.Guid, loser.MainModel!.Nickname, loser.MainModel.Guid);
            return;
        }

        if (winnerPos > loserPos)
        {
            ChangePositionDto change = new ChangePositionDto
            {
                TargetPosition = new PositionModel
                {
                    Tier = winnerPos.Tier,
                    TierPosition = winnerPos.TierPosition
                },
                NewPosition = new PositionModel
                {
                    Tier = loserPos.Tier,
                    TierPosition = loserPos.TierPosition
                },
            };

            _changesContainer.AddChanges([change]);
        }
    }

    private void UpdateWinnerVideo(BattledPersonModel winner)
    {
        (PersonModel winnerModel, PositionModel winnerPos) = winner.GetActual(_tierListContainer.GetHead());

        if (winnerPos == PositionModel.Unknown)
        {
            _logger.LogError("Участник баттла {nick1} ({guid1}) имеет неопределенную позицию", winner.MainModel!.Nickname, winner.MainModel.Guid);
            return;
        }

        winnerModel.VideoLink = winner.VideoUrl!;

        ChangePersonPropertiesDto change = new ChangePersonPropertiesDto
        {
            TargetPosition = new PositionModel
            {
                Tier = winnerPos.Tier,
                TierPosition = winnerPos.TierPosition
            },
            NewProperties = winnerModel,
        };

        _changesContainer.AddChanges([change]);
    }

    public async Task<BattleDatabase> AddBattle(BattleDto battle)
    {
        BattleDatabase model = battle.CreateFrom(_tierListContainer.GetHead());

        if (model.Left.PreBattlePosition == PositionModel.Unknown || model.Left.PreBattlePosition == PositionModel.Unknown)
            throw new InvalidOperationException($"Один/несколько из участников {battle.Left.Guid} и {battle.Right.Guid} не найден/найдены!");

        await _applicationContext.Battles.AddAsync(model);
        await _applicationContext.SaveChangesAsync();

        return model;
    }
}
