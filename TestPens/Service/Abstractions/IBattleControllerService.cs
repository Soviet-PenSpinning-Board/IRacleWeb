using TestPens.Models.Abstractions;
using TestPens.Models;
using TestPens.Models.Simple;

namespace TestPens.Service.Abstractions
{
    public interface IBattleControllerService
    {
        public IReadOnlyDictionary<Guid, BattleModel> GetUnactiveBattles(int offset = 0, int limit = int.MaxValue);

        public IReadOnlyDictionary<Guid, BattleModel> GetActiveBattles();

        public bool ChangeResult(Guid guid, BattleResult battleResult, bool performPositionChange);

        public Guid AddBattle(BattleModel battle);
    }
}
