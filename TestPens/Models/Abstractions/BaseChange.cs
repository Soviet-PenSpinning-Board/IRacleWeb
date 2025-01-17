
using TestPens.Models.Simple;

namespace TestPens.Models.Abstractions
{
    public abstract class BaseChange
    {
        public DateTime UtcTime { get; set; }

        public abstract ChangeType Type { get; set; }

        protected BaseChange(DateTime dateTime)
        {
            UtcTime = dateTime;
        }

        public abstract void Apply(Dictionary<Tier, List<PersonModel>> tierListState);

        public abstract void Revert(Dictionary<Tier, List<PersonModel>> tierListState);
    }

    public enum ChangeType
    {
        None,
        ChangePosition,
        NewPerson,
        PersonProperties
    }
}
