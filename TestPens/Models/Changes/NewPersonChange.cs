
using System;

using TestPens.Models.Abstractions;
using TestPens.Models.Simple;

namespace TestPens.Models.Changes
{
    public class NewPersonChange : BaseChange
    {
        public NewPersonChange() :
            base(DateTime.UtcNow)
        {
        }

        public NewPersonChange(PersonModel person, ShortPositionModel position) :
            base(DateTime.UtcNow)
        {
            Person = person;
            Position = position;
        }

        public PersonModel Person { get; } = null!;
        public ShortPositionModel Position { get; } = null!;

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
