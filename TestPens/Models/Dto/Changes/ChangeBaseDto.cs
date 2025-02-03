
using System.Collections.Generic;
using System.Text.Json.Serialization;

using Swashbuckle.AspNetCore.Annotations;

using TestPens.Models.Real;
using TestPens.Models.Real.Changes;
using TestPens.Models.Shared;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Dto.Changes
{
    public abstract class ChangeBaseDto : IDtoObject<ChangeBaseModel>
    {
        public abstract ChangeType Type { get; set; }

        public PositionModel TargetPosition { get; set; } = null!;

        public virtual bool Validate(TierListState head, out string reason)
        {
            if (!Enum.IsDefined(TargetPosition.Tier))
            {
                reason = $"TargetPosition.Tier указан неправильно! ({TargetPosition.Tier})";
                return false;
            }

            if (!head.TierList.TryGetValue(TargetPosition.Tier, out var list))
            {
                reason = $"TargetPosition.Tier указан неправильно, в теории это сообщение невозможно? ({TargetPosition.Tier})";
                return false;
            }

            if (list.Count <= TargetPosition.TierPosition)
            {
                reason = $"TargetPosition.TierPosition указан неправильно! ({TargetPosition.TierPosition}/{list.Count})";
                return false;
            }

            reason = string.Empty;
            return true;
        }

        public abstract ChangeBaseModel CreateFrom(TierListState head);
    }
}
