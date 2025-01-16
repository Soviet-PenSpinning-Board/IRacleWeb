
using TestPens.Models.Simple;

namespace TestPens.Models.Abstractions
{
    public abstract class BaseChange
    {
        public DateTime UtcTime { get; }

        public Guid Signature { get; }

        protected BaseChange(Guid signature, DateTime dateTime)
        {
            UtcTime = dateTime;
            Signature = signature;
        }

        public abstract void Apply(Dictionary<Tier, List<PersonModel>> tierListState);

        public abstract void Revert(Dictionary<Tier, List<PersonModel>> tierListState);
    }
}
