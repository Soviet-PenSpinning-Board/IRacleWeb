
using System;

using TestPens.Extensions;
using TestPens.Models.Dto;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Real.Changes;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Real.Changes
{
    public class ChangeGlobalPersonModel : ChangeBaseModel
    {
        public override ChangeType Type { get; set; } = ChangeType.GlobalPerson;

        public bool IsNew { get; set; }

        public override void Apply(TierListState state, bool revert)
        {
            List<PersonModel> tier = state.TierList[TargetPosition.Tier];

            if (IsNew != revert)
                tier.Insert(TargetPosition.TierPosition, TargetPerson.Copy());
            else
                tier.RemoveAt(TargetPosition.TierPosition);
        }
    }
}
