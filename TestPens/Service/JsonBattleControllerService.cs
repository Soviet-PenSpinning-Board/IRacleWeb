
using System.Collections.Generic;
using System.Text.Json;

using TestPens.Models;
using TestPens.Models.Abstractions;
using TestPens.Models.Changes;
using TestPens.Models.Simple;

namespace TestPens.Service.Abstractions
{
    public class JsonBattleControllerService : IBattleControllerService
    {
        private Dictionary<Guid, BattleModel>? cachedBattles;

        private readonly ILogger<JsonBattleControllerService> _logger;
        private readonly IConfiguration configuration;
        private readonly IPersonContainerService personContainer;

        private string BattlesPath => configuration.GetValue<string>("BattlesPath")
                    ?? throw new NullReferenceException("Конфигурация BattlesPath не задана!");

        public JsonBattleControllerService(ILogger<JsonBattleControllerService> logger, IConfiguration configuration, IPersonContainerService personContainer)
        {
            _logger = logger;
            this.configuration = configuration;
            this.personContainer = personContainer;
        }

        public IReadOnlyDictionary<Guid, BattleModel> GetAllBattles(int offset = 0, int limit = int.MaxValue, DateTime? afterTime = null!)
        {
            EnsureCachedBattles();

            var enumerable = afterTime != null ? cachedBattles!.Where(c => c.Value.UtcTime > afterTime) : cachedBattles!;

            return enumerable.Skip(offset).Take(limit).ToDictionary();
        }

        public void EnsureCachedBattles()
        {
            if (cachedBattles != null)
                return;

            if (!File.Exists(BattlesPath))
            {
                File.WriteAllText(BattlesPath, "{}");
                cachedBattles = new();
                return;
            }

            string content = File.ReadAllText(BattlesPath);
            cachedBattles = JsonSerializer.Deserialize<Dictionary<Guid, BattleModel>>(content, Program.JsonOptions)!;
        }

        public IReadOnlyDictionary<Guid, BattleModel> GetActiveBattles()
        {
            EnsureCachedBattles();
            return cachedBattles!.Where(battle => battle.Value.Result == BattleResult.Unfinished).ToDictionary();
        }

        public bool ChangeResult(Guid guid, BattleResult battleResult, bool performPositionChange)
        {
            EnsureCachedBattles();

            if (!cachedBattles!.TryGetValue(guid, out BattleModel? battle))
            {
                _logger.LogError("Битва: {guid} не найдена, метод {name}", guid, nameof(ChangeResult));
                return false;
            }

            battle.Result = battleResult;

            if (performPositionChange)
            {
                if (battleResult is BattleResult.RightWin)
                {
                    TryChangeBattlePositions(battle.Right, battle.Left);
                }
                if (battleResult is BattleResult.LeftWin)
                {
                    TryChangeBattlePositions(battle.Left, battle.Right);
                }
            }

            Save();

            return true;
        }

        private void TryChangeBattlePositions(BattledPersonModel winner, BattledPersonModel loser)
        {
            TierListState head = personContainer.GetHead();
            (_, ShortPositionModel winnerPos) = winner.GetActualProperties(head);
            (_, ShortPositionModel loserPos) = loser.GetActualProperties(head);

            if (winnerPos == ShortPositionModel.Unknown || loserPos == ShortPositionModel.Unknown)
            {
                _logger.LogError("Один из участников баттла между {nick1} ({guid1}) и {nick2} ({guid2}) имеет неопределенную позицию", winner.MainModel.Nickname, winner.MainModel.Guid, loser.MainModel.Nickname, loser.MainModel.Guid);
                return;
            }
            if (winnerPos > loserPos)
            {
                PositionChange change = new PositionChange
                {
                    NewPosition = loserPos,
                    OldPosition = winnerPos
                };
                personContainer.AddChanges([change]);
            }
        }

        public Guid AddBattle(BattleModel battle)
        {
            EnsureCachedBattles();
            battle.Initialize(personContainer.GetHead());
            Guid guid = Guid.NewGuid();
            cachedBattles!.Add(guid, battle);

            Save();

            return guid;
        }

        public void Save()
        {
            var battles = GetAllBattles();
            File.WriteAllText(BattlesPath, JsonSerializer.Serialize(battles, Program.JsonOptions));
        }
    }
}
