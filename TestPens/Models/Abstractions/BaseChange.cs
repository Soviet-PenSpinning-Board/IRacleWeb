
using System.Text.Json.Serialization;

using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Abstractions
{
    public abstract class BaseChange
    {
        public DateTime UtcTime { get; set; }

        public abstract ChangeType Type { get; set; }

        public virtual void Initialize(Dictionary<Tier, List<PersonModel>> head) { }

        public virtual bool IsAffective() => true;

        protected BaseChange(DateTime dateTime)
        {
            UtcTime = dateTime;
        }

        public abstract Permissions GetPermission();

        public abstract void Apply(Dictionary<Tier, List<PersonModel>> tierListState);

        public abstract BaseChange RevertedChange();
    }

    public enum ChangeType
    {
        None,
        ChangePosition,
        GlobalPerson,
        PersonProperties
    }
}
