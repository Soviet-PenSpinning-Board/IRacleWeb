using Microsoft.AspNetCore.Html;

using TestPens.Models.Dto;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Real.Changes;
using TestPens.Models.Shared;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Real.Changes
{
    public class ChangeNoneModel : ChangeBaseModel
    {
        public override ChangeType Type { get; set; } = ChangeType.None;

        public override void Apply(TierListState state, bool revert) { }

        public override GenericChangeDatabase ToGeneric(ulong chunk)
        {
            return new GenericChangeDatabase 
            { 
            };
        }

        public override void ReadData(GenericChangeDatabase genericChange)
        {
        }

        public override string GetIcon() =>
            "/icons/default.png";

        public override string LocalizeName() =>
            "Ничего..?";

        public override string LocalizeDescription() => "Как оно тут оказалось...";

        public override PersonModel LocalizeTarget() => TargetPerson!;
    }
}
