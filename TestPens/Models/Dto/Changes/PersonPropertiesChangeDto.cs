
using System;

using TestPens.Extensions;
using TestPens.Models.Dto;
using TestPens.Models.Real;
using TestPens.Models.Real.Changes;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Dto.Changes
{
    public class PersonPropertiesChangeDto : ChangeBaseDto
    {
        public override ChangeType Type { get; set; } = ChangeType.PersonProperties;

        public PersonDto NewProperties { get; set; } = null!;

        public override PersonPropertiesChangeModel CreateFrom(TierListState head)
        {
            PositionModel position = TargetPosition.CreateFrom(head);
            return new PersonPropertiesChangeModel
            {
                UtcTime = DateTime.UtcNow,
                TargetPerson = position.GetPerson(head)!,
                TargetPosition = position,
                NewProperties = NewProperties.CreateFrom(head),
            };
        }
    }
}
