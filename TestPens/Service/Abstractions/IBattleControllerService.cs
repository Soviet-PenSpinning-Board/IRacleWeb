using System.Collections;

using TestPens.Models;
using TestPens.Models.Dto;
using TestPens.Models.Real;
using TestPens.Models.Simple;

namespace TestPens.Service.Abstractions
{
    public interface IBattleControllerService
    {
        public Task<IEnumerable<BattleDatabase>> GetUnactiveBattles(int offset = 0, int limit = int.MaxValue);

        public Task<IEnumerable<BattleDatabase>> GetActiveBattles();

        public Task<bool> ChangeResult(Guid guid, BattleResult battleResult, bool performPositionChange);

        public Task<BattleDatabase> AddBattle(BattleDto battle);
    }
}
