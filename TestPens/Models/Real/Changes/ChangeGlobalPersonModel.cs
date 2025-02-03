
using System;

using TestPens.Extensions;
using TestPens.Models.Dto;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Real.Changes;
using TestPens.Models.Shared;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Real.Changes
{
    public class ChangeGlobalPersonModel : ChangeBaseModel
    {
        public override ChangeType Type { get; set; } = ChangeType.GlobalPerson;

        public PersonModel? NewPerson { get; set; }

        public override void Apply(TierListState state, bool revert)
        {
            List<PersonModel> tier = state.TierList[TargetPosition.Tier];

            PersonModel? person = revert ? TargetPerson : NewPerson;

            if (person != null)
                tier.Insert(TargetPosition.TierPosition, person!.Copy());
            else
                tier.RemoveAt(TargetPosition.TierPosition);
        }

        public override GenericChangeDatabase ToGeneric(ulong chunk)
        {
            return new GenericChangeDatabase
            {
                UtcTime = UtcTime,
                Type = ChangeType.GlobalPerson,
                Chunk = chunk,
                Data = new GenericChangeDatabase.ExtraData
                {
                    TargetPerson = TargetPerson,
                    TargetPosition = TargetPosition,
                    NewPerson = NewPerson,
                }
            };
        }

        public override void ReadData(GenericChangeDatabase genericChange)
        {
            UtcTime = genericChange.UtcTime;
            TargetPosition = genericChange.Data.TargetPosition;
            TargetPerson = genericChange.Data.TargetPerson;
            NewPerson = genericChange.Data.NewPerson;
        }
    }
}
