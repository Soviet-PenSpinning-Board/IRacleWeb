using TestPens.Models.Dto;
using TestPens.Models.Real;
using TestPens.Models.Real.Changes;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Dto.Changes
{
    public class NoneChangeDto : ChangeBaseDto
    {
        public override ChangeType Type { get; set; } = ChangeType.None;

        public override ChangeBaseModel CreateFrom(TierListState head)
        {
            PositionModel position = TargetPosition.CreateFrom(head);
            return new NoneChangeModel
            {
                UtcTime = DateTime.UtcNow,
                TargetPerson = position.GetPerson(head)!,
                TargetPosition = position,
            };
        }
    }
}
