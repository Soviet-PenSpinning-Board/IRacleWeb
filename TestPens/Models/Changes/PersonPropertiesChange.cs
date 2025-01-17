
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

        public PersonPropertiesChange(ShortPositionModel position, PersonModel old, PersonModel @new) :
            base(DateTime.UtcNow)
        {
            Position = position;
            OldProperties = old;
            NewProperties = @new;
        }

        public override ChangeType Type { get; set; } = ChangeType.PersonProperties;

        public ShortPositionModel Position { get; set; } = null!;
        public PersonModel OldProperties { get; set; } = null!;
        public PersonModel NewProperties { get; set; } = null!;

        public override Permissions GetPermission() =>
            Permissions.ChangeProperties;

        public override void Apply(Dictionary<Tier, List<PersonModel>> tierListState) =>
            Generic(tierListState, NewProperties);

        public override void Revert(Dictionary<Tier, List<PersonModel>> tierListState) =>
            Generic(tierListState, OldProperties);

        public void Generic(Dictionary<Tier, List<PersonModel>> tierListState, PersonModel @new)
        {
            List<PersonModel> tier = tierListState[Position.Tier];

            tier.RemoveAt(Position.TierPosition);
            tier.Insert(Position.TierPosition, @new);
        }
    }
}
