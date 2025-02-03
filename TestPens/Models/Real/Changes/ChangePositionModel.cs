using TestPens.Models.Dto;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Real.Changes;
using TestPens.Models.Shared;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Real.Changes
{
    public class ChangePositionModel : ChangeBaseModel
    {
        public override ChangeType Type { get; set; } = ChangeType.ChangePosition;

        public PositionModel NewPosition { get; set; } = null!;

        public override bool IsAffective()
        {
            return NewPosition != TargetPosition;
        }

        public override void Apply(TierListState state, bool revert)
        {
            (PositionModel oldPosition, PositionModel newPosition) = !revert ? (TargetPosition, NewPosition) : (NewPosition, TargetPosition);

            List<PersonModel> oldTier = state.TierList[oldPosition.Tier];
            List<PersonModel> newTier = state.TierList[newPosition.Tier];

            PersonModel personModel = oldTier[oldPosition.TierPosition];
            oldTier.RemoveAt(oldPosition.TierPosition);

            newTier.Insert(newPosition.TierPosition, personModel);
        }

        public override GenericChangeDatabase ToGeneric(ulong chunk)
        {
            return new GenericChangeDatabase
            {
                UtcTime = UtcTime,
                Type = ChangeType.ChangePosition,
                Chunk = chunk,
                Data = new GenericChangeDatabase.ExtraData
                {
                    TargetPerson = TargetPerson,
                    TargetPosition = TargetPosition,
                    NewPosition = NewPosition,
                }
            };
        }

        public override void ReadData(GenericChangeDatabase genericChange)
        {
            UtcTime = genericChange.UtcTime;
            TargetPosition = genericChange.Data.TargetPosition;
            TargetPerson = genericChange.Data.TargetPerson;
            NewPosition = genericChange.Data.NewPosition!;
        }
    }
}
