
using System.Text.Json.Serialization;

using TestPens.Models.Dto.Changes;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Real.Changes
{
    public abstract class ChangeBaseModel : IModelObject<ChangeBaseDto>
    {
        public abstract ChangeType Type { get; set; }

        public DateTime UtcTime { get; set; }

        public PersonModel TargetPerson { get; set; } = null!;

        public PositionModel TargetPosition { get; set; } = null!;

        public virtual bool IsAffective() => true;

        public abstract void Apply(TierListState state, bool revert);

        public abstract ChangeBaseDto ToForm();
    }
}
