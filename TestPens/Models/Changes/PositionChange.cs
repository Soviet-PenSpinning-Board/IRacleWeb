
using TestPens.Models.Abstractions;
using TestPens.Models.Simple;

namespace TestPens.Models.Changes
{
    public class PositionChange : BaseChange
    {
        public PositionChange() :
            base(Guid.NewGuid(), DateTime.UtcNow)
        {
        }

        public PositionChange(ShortPositionModule newPosition, ShortPositionModule oldPosition) :
            base(Guid.NewGuid(), DateTime.UtcNow)
        {
            NewPosition = newPosition;
            OldPosition = oldPosition;
        }

        public override ChangeType Type { get; set; } = ChangeType.ChangePosition;

        public ShortPositionModule NewPosition { get; } = null!;
        public ShortPositionModule OldPosition { get; } = null!;

        public override void Apply(Dictionary<Tier, List<PersonModel>> tierListState) =>
            Generic(tierListState, OldPosition, NewPosition);

        // оказывается для реверта изменения позиции достаточно просто поменять местами старую и новую позиции, я понял это только через пару часов...
        public override void Revert(Dictionary<Tier, List<PersonModel>> tierListState) =>
            Generic(tierListState, NewPosition, OldPosition);

        public void Generic(Dictionary<Tier, List<PersonModel>> tierListState, ShortPositionModule oldPosition, ShortPositionModule newPosition)
        {
            List<PersonModel> oldTier = tierListState[oldPosition.Tier];
            List<PersonModel> newTier = tierListState[newPosition.Tier];

            PersonModel personModel = oldTier[oldPosition.TierPosition];
            oldTier.RemoveAt(oldPosition.TierPosition);

            // случай когда человек опускается в рамках своего тира ниже своей изначальнйо позиции, индекс сдвигается на 1, поэтому надо утчитывать
            if (oldPosition.Tier == newPosition.Tier && newPosition.TierPosition > oldPosition.TierPosition)
            {
                newTier.Insert(newPosition.TierPosition - 1, personModel);
                return;
            }
            newTier.Insert(newPosition.TierPosition, personModel);
        }
    }
}
