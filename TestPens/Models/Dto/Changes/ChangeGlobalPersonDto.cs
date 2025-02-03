
using System;

using TestPens.Extensions;
using TestPens.Models.Real;
using TestPens.Models.Real.Changes;
using TestPens.Models.Shared;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Dto.Changes
{
    public class ChangeGlobalPersonDto : ChangeBaseDto
    {
        public override ChangeType Type { get; set; } = ChangeType.GlobalPerson;

        public PersonModel? NewPerson { get; set; }

        public bool IsNew { get; set; }

        public override bool Validate(TierListState head, out string reason)
        {
            if (!IsNew)
                return base.Validate(head, out reason);

            if (NewPerson == null)
            {
                reason = "$\"NewPerson\" указан неправильно!";
                return false;
            }

            reason = "";
            return true;
        }

        public override ChangeBaseModel CreateFrom(TierListState head)
        {
            PositionModel position;
            PersonModel? oldPerson = null;
            PersonModel? newPerson = null;
            if (IsNew)
            {
                position = new PositionModel
                {
                    Tier = Tier.E,
                    TierPosition = head.TierList[Tier.E].Count,
                };
                newPerson = NewPerson!;
                newPerson.Guid = Guid.NewGuid();
            }
            else
            {
                position = TargetPosition;
                oldPerson = position.GetPerson(head)!.Copy();
            }

            return new ChangeGlobalPersonModel
            {
                UtcTime = DateTime.UtcNow,
                TargetPerson = oldPerson,
                TargetPosition = position,
                NewPerson = newPerson,
            };
        }
    }
}
