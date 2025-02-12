
using System;

using TestPens.Extensions;
using TestPens.Models.Real;
using TestPens.Models.Real.Changes;
using TestPens.Models.Shared;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Dto.Changes
{
    public class ChangePersonPropertiesDto : ChangeBaseDto
    {
        public override ChangeType Type { get; set; } = ChangeType.PersonProperties;

        public PersonModel NewProperties { get; set; } = null!;

        public override ChangePersonPropertiesModel CreateFrom(TierListState head)
        {
            return new ChangePersonPropertiesModel
            {
                UtcTime = DateTime.UtcNow,
                TargetPerson = TargetPosition.GetPerson(head)!.Copy(),
                TargetPosition = TargetPosition,
                NewProperties = NewProperties.CreateFrom(head),
            };
        }
    }
}
