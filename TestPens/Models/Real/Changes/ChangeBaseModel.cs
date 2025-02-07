
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Html;

using Swashbuckle.AspNetCore.Annotations;

using TestPens.Models.Dto.Changes;
using TestPens.Models.Shared;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Real.Changes
{
    public abstract class ChangeBaseModel
    {
        public abstract ChangeType Type { get; set; }

        public DateTime UtcTime { get; set; }

        public PersonModel? TargetPerson { get; set; } = null!;

        public PositionModel TargetPosition { get; set; } = null!;

        public virtual bool IsAffective() => true;

        public abstract void Apply(TierListState state, bool revert);

        public abstract GenericChangeDatabase ToGeneric(ulong chunk);

        public abstract void ReadData(GenericChangeDatabase genericChange);

        public abstract string GetIcon();

        public abstract string LocalizeName();

        public abstract string LocalizeDescription();

        public abstract PersonModel LocalizeTarget();

        public static ChangeBaseModel? Create(GenericChangeDatabase genericChange)
        {
            if (genericChange.Type.GetModelType() is not Type type)
            {
                return null;
            }

            ChangeBaseModel change = (ChangeBaseModel)Activator.CreateInstance(type)!;
            change.ReadData(genericChange);
            return change;
        }
    }
}
