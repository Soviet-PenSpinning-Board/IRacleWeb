
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

        public ShortPositionModel Position { get; set; } = null!;
        public PersonModel OldProperties { get; set; } = null!;
        public PersonModel NewProperties { get; set; } = null!;

        public override bool IsAffective()
        {
            return OldProperties != NewProperties;
        }

        public override void Initialize(Dictionary<Tier, List<PersonModel>> head)
        {
            base.Initialize(head);
            if (OldProperties == null)
                OldProperties = head[Position.Tier][Position.TierPosition];
        }

        public override Permissions GetPermission() =>
            Permissions.ChangeProperties;

        public override void Apply(Dictionary<Tier, List<PersonModel>> tierListState)
        {
            PersonModel person = tierListState[Position.Tier][Position.TierPosition];

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
                Position = Position,
                OldProperties = NewProperties,
                NewProperties = OldProperties,
            };
        }
    }
}
