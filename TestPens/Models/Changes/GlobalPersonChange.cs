
using System;

using TestPens.Models.Abstractions;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Changes
{
    public class GlobalPersonChange : BaseChange
    {
        public GlobalPersonChange() :
            base(DateTime.UtcNow)
        {
        }

        public GlobalPersonChange(PersonModel person, ShortPositionModel position, bool isNew) :
            base(DateTime.UtcNow)
        {
            Person = person;
            Position = position;
            IsNew = isNew;
        }

        public PersonModel Person { get; set; } = null!;
        public ShortPositionModel Position { get; set; } = null!;

        public bool IsNew { get; set; }

        public override ChangeType Type { get; set; } = ChangeType.GlobalPerson;

        public override Permissions GetPermission() =>
             Permissions.GlobalMember;

        public override void Apply(Dictionary<Tier, List<PersonModel>> tierListState)
        {
            List<PersonModel> tier = tierListState[Position.Tier];

            if (IsNew)
                tier.Insert(Position.TierPosition, Person);
            else
                tier.RemoveAt(Position.TierPosition);
        }

        public override BaseChange RevertedChange()
        {
            return new GlobalPersonChange
            {
                UtcTime = UtcTime,
                Position = Position,
                Person = Person,
                IsNew = !IsNew,
            };
        }
    }
}
