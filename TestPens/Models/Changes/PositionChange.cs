
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

        public override ChangeType Type { get; set; } = ChangeType.ChangePosition;

        public ShortPositionModel NewPosition { get; set; } = null!;

        public override bool IsAffective()
        {
            return NewPosition != TargetPosition;
        }

        public override void Initialize(Dictionary<Tier, List<PersonModel>> head)
        {
            base.Initialize(head);
        }

        public override Permissions GetPermission() =>
            Permissions.ChangePositions;

        public override void Apply(Dictionary<Tier, List<PersonModel>> tierListState)
        {
            List<PersonModel> oldTier = tierListState[TargetPosition.Tier];
            List<PersonModel> newTier = tierListState[NewPosition.Tier];

            PersonModel personModel = oldTier[TargetPosition.TierPosition];
            oldTier.RemoveAt(TargetPosition.TierPosition);

            newTier.Insert(NewPosition.TierPosition, personModel);
        }

        public override BaseChange RevertedChange()
        {
            return new PositionChange
            {
                UtcTime = UtcTime,
                TargetPosition = TargetPosition,
                TargetPerson = TargetPerson,
                NewPosition = TargetPosition,
            };
        }
    }
}
