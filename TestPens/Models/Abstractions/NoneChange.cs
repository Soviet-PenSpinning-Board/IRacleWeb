
using TestPens.Models.Simple;

namespace TestPens.Models.Abstractions
{
    public class NoneChange : BaseChange
    {
        public override ChangeType Type { get; set; } = ChangeType.None;

        public NoneChange()
            : base(DateTime.UtcNow)
        {
        }

        public override void Apply(Dictionary<Tier, List<PersonModel>> tierListState) { }

        public override void Revert(Dictionary<Tier, List<PersonModel>> tierListState) { }
    }
}
