
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using TestPens.Models;
using TestPens.Models.Dto;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Real;
using TestPens.Models.Real.Changes;
using TestPens.Models.Simple;

namespace TestPens.Service.Abstractions
{
    public class JsonBattleControllerService : IBattleControllerService
    {
        private Dictionary<Guid, BattleModel>? cachedBattles;

        private readonly ILogger<JsonBattleControllerService> _logger;
        private readonly IConfiguration configuration;
        private readonly IPersonContainerService personContainer;

        private string BattlesPath { get; }

        public JsonBattleControllerService(ILogger<JsonBattleControllerService> logger, IConfiguration configuration, IPersonContainerService personContainer)
        {
            _logger = logger;
            this.configuration = configuration;
            this.personContainer = personContainer;

            BattlesPath = Path.Combine(configuration.GetValue<string>("ConfigPath")!, "battles.json");
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

        public IReadOnlyDictionary<Guid, BattleModel> GetUnactiveBattles(int offset = 0, int limit = int.MaxValue)
        {
            return GetPredicate(battle => battle.Result != BattleResult.Unfinished).Skip(offset).Take(limit).ToDictionary();
        }

        public IReadOnlyDictionary<Guid, BattleModel> GetActiveBattles()
        {
            return GetPredicate(battle => battle.Result == BattleResult.Unfinished).ToDictionary();
        }

        private IEnumerable<KeyValuePair<Guid, BattleModel>> GetPredicate(Predicate<BattleModel> predicate)
        {
            EnsureCachedBattles();
            return cachedBattles!.Where(b => predicate(b.Value)).Reverse();
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
            (_, PositionModel winnerPos) = winner.GetActual(head);
            (_, PositionModel loserPos) = loser.GetActual(head);

            if (winnerPos == PositionModel.Unknown || loserPos == PositionModel.Unknown)
            {
                _logger.LogError("Один из участников баттла между {nick1} ({guid1}) и {nick2} ({guid2}) имеет неопределенную позицию", winner.MainModel!.Nickname, winner.MainModel.Guid, loser.MainModel!.Nickname, loser.MainModel.Guid);
                return;
            }

            if (winnerPos > loserPos)
            {
                PositionChangeDto change = new PositionChangeDto
                {
                    TargetPosition = winnerPos.ToForm(),
                    NewPosition = loserPos.ToForm(),
                };

                personContainer.AddChanges([change]);
            }
        }

        public Guid AddBattle(BattleDto battle)
        {
            EnsureCachedBattles();
            BattleModel model = battle.CreateFrom(personContainer.GetHead());

            if (model.Left.PreBattlePosition == PositionModel.Unknown || model.Left.PreBattlePosition == PositionModel.Unknown)
                throw new InvalidOperationException($"Один/несколько из участников {battle.Left.Guid} и {battle.Right.Guid} не найден/найдены!");
            
            Guid guid = Guid.NewGuid();
            cachedBattles!.Add(guid, model);

            Save();

            return guid;
        }

        public void Save()
        {
            EnsureCachedBattles();
            File.WriteAllText(BattlesPath, JsonSerializer.Serialize(cachedBattles, Program.JsonOptions));
        }
    }
}
