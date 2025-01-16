
using TestPens.Models.Abstractions;
using TestPens.Models.Simple;

namespace TestPens.Models.Changes
{
    public class BattleResultChange : BaseChange
    {
        public BattleResultChange() :
            base(Guid.NewGuid(), DateTime.UtcNow)
        {
            
        }

        public BattleResultChange(ShortPersonModule winner, ShortPersonModule loser) :
            base(Guid.NewGuid(), DateTime.UtcNow)
        {
            Winner = winner;
            Loser = loser;
        }

        public BattleResultChange(PersonModel winner, PersonModel loser, DateTime time, Guid signature)
            : base(signature, time)
        {
            Winner = winner;
            Loser = loser;
        }

        public ShortPersonModule Winner { get; }
        public ShortPersonModule Loser { get; }

        public override void Apply(Dictionary<Tier, List<PersonModel>> tierListState)
        {
            ti
        }

        public override void Revert(Dictionary<Tier, List<PersonModel>> tierListState)
        {
            throw new NotImplementedException();
        }
    }
}
