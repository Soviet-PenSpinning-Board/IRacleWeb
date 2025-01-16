
using System;

using TestPens.Models.Abstractions;
using TestPens.Models.Simple;

namespace TestPens.Models.Changes
{
    public class PersonActiveChange : BaseChange
    {
        public PersonActiveChange() :
            base(Guid.NewGuid(), DateTime.UtcNow)
        {
        }

        public PersonActiveChange(ShortPositionModule position) :
            base(Guid.NewGuid(), DateTime.UtcNow)
        {
            Position = position;
        }

        public override ChangeType Type { get; set; } = ChangeType.Drop;

        public ShortPositionModule Position { get; } = null!;

        public override void Apply(Dictionary<Tier, List<PersonModel>> tierListState)
        {
            List<PersonModel> tier = tierListState[Position.Tier];

            PersonModel person = tier[Position.TierPosition];
            person.InDrop = !person.InDrop;
        }

        public override void Revert(Dictionary<Tier, List<PersonModel>> tierListState) =>
            Apply(tierListState);
    }
}
