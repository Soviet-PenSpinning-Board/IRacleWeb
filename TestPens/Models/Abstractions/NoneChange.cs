
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Abstractions
{
    public class NoneChange : BaseChange
    {
        public override ChangeType Type { get; set; } = ChangeType.None;

        public NoneChange()
            : base(DateTime.UtcNow)
        {
        }

        public override Permissions GetPermission() => 
            Permissions.None;

        public override void Apply(Dictionary<Tier, List<PersonModel>> tierListState) { }

        public override BaseChange RevertedChange() => this;
    }
}
