
using TestPens.Models.Abstractions;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Changes
{
    public class PositionChange : BaseChange
    {
        public PositionChange() :
            base(DateTime.UtcNow)
        {
        }

        public PositionChange(ShortPositionModel newPosition, ShortPositionModel oldPosition) :
            base(DateTime.UtcNow)
        {
            NewPosition = newPosition;
            OldPosition = oldPosition;
        }

        public override ChangeType Type { get; set; } = ChangeType.ChangePosition;

        public ShortPositionModel NewPosition { get; set; } = null!;
        public ShortPositionModel OldPosition { get; set; } = null!;

        public PersonModel Person { get; set; } = null!;

        public override bool IsAffective()
        {
            return NewPosition != OldPosition;
        }

        public override void Initialize(Dictionary<Tier, List<PersonModel>> head)
        {
            base.Initialize(head);
            if (Person == null)
                Person = head[OldPosition.Tier][OldPosition.TierPosition];
        }

        public override Permissions GetPermission() =>
            Permissions.ChangePositions;

        public override void Apply(Dictionary<Tier, List<PersonModel>> tierListState)
        {
            List<PersonModel> oldTier = tierListState[OldPosition.Tier];
            List<PersonModel> newTier = tierListState[NewPosition.Tier];

            PersonModel personModel = oldTier[OldPosition.TierPosition];
            oldTier.RemoveAt(OldPosition.TierPosition);

            newTier.Insert(NewPosition.TierPosition, personModel);
        }

        public override BaseChange RevertedChange()
        {
            return new PositionChange
            {
                UtcTime = UtcTime,
                Person = Person,
                NewPosition = OldPosition,
                OldPosition = NewPosition,
            };
        }
    }
}
