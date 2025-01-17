
using System;

using TestPens.Models.Abstractions;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Changes
{
    public class DeletePersonChange : BaseChange
    {
        public DeletePersonChange() :
            base(DateTime.UtcNow)
        {
        }

        public DeletePersonChange(PersonModel person, ShortPositionModel position) :
            base(DateTime.UtcNow)
        {
            Person = person;
            Position = position;
        }

        public PersonModel Person { get; set; } = null!;
        public ShortPositionModel Position { get; set; } = null!;

        public override ChangeType Type { get; set; } = ChangeType.NewPerson;

        public override Permissions GetPermission() =>
             Permissions.DeleteMember;

        public override void Apply(Dictionary<Tier, List<PersonModel>> tierListState)
        {
            List<PersonModel> tier = tierListState[Position.Tier];

            tier.RemoveAt(Position.TierPosition);
        }

        public override void Revert(Dictionary<Tier, List<PersonModel>> tierListState)
        {
            List<PersonModel> tier = tierListState[Position.Tier];

            tier.Insert(Position.TierPosition, Person);
        }
    }
}
