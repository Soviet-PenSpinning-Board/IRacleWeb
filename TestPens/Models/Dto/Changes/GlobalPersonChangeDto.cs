
using System;

using TestPens.Extensions;
using TestPens.Models.Dto;
using TestPens.Models.Real;
using TestPens.Models.Real.Changes;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Dto.Changes
{
    public class GlobalPersonChangeDto : ChangeBaseDto
    {
        public override ChangeType Type { get; set; } = ChangeType.GlobalPerson;

        public bool IsNew { get; set; }

        public override bool Validate(TierListState head, out string reason)
        {
            if (!IsNew)
                return base.Validate(head, out reason);

            if (TargetPerson == null)
            {
                reason = "$\"TargetPerson\" указан неправильно!";
                return false;
            }

            reason = "";
            return true;
        }

        public override ChangeBaseModel CreateFrom(TierListState head)
        {
            PositionModel position;
            PersonModel person;
            if (IsNew)
            {
                position = new PositionModel
                {
                    Tier = Tier.E,
                    TierPosition = head.TierList[Tier.E].Count,
                };
                person = TargetPerson!.CreateFrom(head);
                person.Guid = Guid.NewGuid();
            }
            else
            {
                position = TargetPosition.CreateFrom(head);
                person = position.GetPerson(head)!;
            }

            return new GlobalPersonChangeModel
            {
                UtcTime = DateTime.UtcNow,
                TargetPerson = person,
                TargetPosition = position,
                IsNew = IsNew,
            };
        }
    }
}
