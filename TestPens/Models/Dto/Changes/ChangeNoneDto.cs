using TestPens.Models.Dto;
using TestPens.Models.Real;
using TestPens.Models.Real.Changes;
using TestPens.Models.Shared;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Dto.Changes
{
    public class ChangeNoneDto : ChangeBaseDto
    {
        public override ChangeType Type { get; set; } = ChangeType.None;

        public override ChangeBaseModel CreateFrom(TierListState head)
        {
            return new ChangeNoneModel
            {
                UtcTime = DateTime.UtcNow,
                TargetPerson = TargetPosition.GetPerson(head)!,
                TargetPosition = TargetPosition,
            };
        }
    }
}
