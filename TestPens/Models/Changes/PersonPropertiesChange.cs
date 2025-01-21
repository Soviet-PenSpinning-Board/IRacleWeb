
using System;

using TestPens.Models.Abstractions;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Changes
{
    public class PersonPropertiesChange : BaseChange
    {
        public PersonPropertiesChange() :
            base(DateTime.UtcNow)
        {
        }

        public override ChangeType Type { get; set; } = ChangeType.PersonProperties;

        public PersonModel NewProperties { get; set; } = null!;

        public override bool IsAffective()
        {
            return TargetPerson != NewProperties;
        }

        public override Permissions GetPermission() =>
            Permissions.ChangeProperties;

        public override void Apply(Dictionary<Tier, List<PersonModel>> tierListState)
        {
            PersonModel person = tierListState[TargetPosition.Tier][TargetPosition.TierPosition];

            person.Nickname = NewProperties.Nickname.Trim();
            person.AvatarUrl = NewProperties.AvatarUrl;
            person.InDrop = NewProperties.InDrop;
            person.VideoLink = NewProperties.VideoLink;
        }

        public override BaseChange RevertedChange()
        {
            return new PersonPropertiesChange
            {
                UtcTime = UtcTime,
                TargetPosition = TargetPosition,
                TargetPerson = NewProperties,
                NewProperties = TargetPerson!,
            };
        }
    }
}
