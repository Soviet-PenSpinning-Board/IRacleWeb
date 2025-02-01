
using System;

using TestPens.Extensions;
using TestPens.Models.Dto;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Real.Changes;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Real.Changes
{
    public class ChangePersonPropertiesModel : ChangeBaseModel
    {
        public override ChangeType Type { get; set; } = ChangeType.PersonProperties;

        public PersonModel NewProperties { get; set; } = null!;

        public override void Apply(TierListState state, bool revert)
        {
            PersonModel person = state.TierList[TargetPosition.Tier][TargetPosition.TierPosition];
            PersonModel fromSet = !revert ? NewProperties : TargetPerson;

            person.Nickname = fromSet.Nickname;
            person.AvatarUrl = fromSet.AvatarUrl;
            person.InDrop = fromSet.InDrop;
            person.VideoLink = fromSet.VideoLink;
            person.Description = fromSet.Description;
        }
    }
}
