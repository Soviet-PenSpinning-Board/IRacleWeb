
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

        public override void Apply(Dictionary<Tier, List<PersonModel>> tierListState)
        {
            List<PersonModel> oldTier = tierListState[OldPosition.Tier];
            List<PersonModel> newTier = tierListState[NewPosition.Tier];

            PersonModel personModel = oldTier[OldPosition.TierPosition];
            oldTier.RemoveAt(OldPosition.TierPosition);
            newTier.Insert(NewPosition.TierPosition, personModel);
        }

        public override void Revert(Dictionary<Tier, List<PersonModel>> tierListState)
        {
            List<PersonModel> oldTier = tierListState[OldPosition.Tier];
            List<PersonModel> newTier = tierListState[NewPosition.Tier];

            PersonModel personModel = newTier[NewPosition.TierPosition];
            newTier.RemoveAt(NewPosition.TierPosition);
            oldTier.Insert(OldPosition.TierPosition, personModel);
        }
    }
}
