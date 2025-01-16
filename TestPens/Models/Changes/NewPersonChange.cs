
using System;

using TestPens.Models.Abstractions;
using TestPens.Models.Simple;

namespace TestPens.Models.Changes
{
    public class NewPersonChange : BaseChange
    {
        public NewPersonChange() :
            base(Guid.NewGuid(), DateTime.UtcNow)
        {
        }

        public NewPersonChange(PersonModel person, ShortPositionModule position) :
            base(Guid.NewGuid(), DateTime.UtcNow)
        {
            Person = person;
            Position = position;
        }

        public PersonModel Person { get; } = null!;
        public ShortPositionModule Position { get; } = null!;

        public override ChangeType Type { get; set; } = ChangeType.NewPerson;

        public override void Apply(Dictionary<Tier, List<PersonModel>> tierListState)
        {
            List<PersonModel> tier = tierListState[Position.Tier];

            tier.Insert(Position.TierPosition, Person);
        }

        public override void Revert(Dictionary<Tier, List<PersonModel>> tierListState)
        {
            List<PersonModel> tier = tierListState[Position.Tier];

            tier.RemoveAt(Position.TierPosition);
        }
    }
}
