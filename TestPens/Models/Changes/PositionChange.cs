
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

        public override Permissions GetPermission() =>
            Permissions.ChangePositions;

        public override void Apply(Dictionary<Tier, List<PersonModel>> tierListState) =>
            Generic(tierListState, OldPosition, NewPosition);

        // оказывается для реверта изменения позиции достаточно просто поменять местами старую и новую позиции, я понял это только через пару часов...
        public override void Revert(Dictionary<Tier, List<PersonModel>> tierListState) =>
            Generic(tierListState, NewPosition, OldPosition);

        public void Generic(Dictionary<Tier, List<PersonModel>> tierListState, ShortPositionModel oldPosition, ShortPositionModel newPosition)
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
