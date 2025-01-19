using TestPens.Models.Abstractions;
using TestPens.Models;
using TestPens.Models.Simple;

namespace TestPens.Service.Abstractions
{
    public interface IBattleControllerService
    {
        public IReadOnlyDictionary<Guid, BattleModel> GetAllBattles(int offset = 0, int limit = int.MaxValue, DateTime? afterTime = null!);

        public IReadOnlyDictionary<Guid, BattleModel> GetActiveBattles();

        public bool ChangeResult(Guid guid, BattleResult battleResult, bool performPositionChange);

        public Guid AddBattle(BattleModel battle);
    }
}
