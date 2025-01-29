using TestPens.Models.Dto;
using TestPens.Models.Real;
using TestPens.Models.Real.Changes;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Dto.Changes
{
    public class PositionChangeDto : ChangeBaseDto
    {
        public override ChangeType Type { get; set; } = ChangeType.ChangePosition;

        public PositionDto NewPosition { get; set; } = null!;

        public override PositionChangeModel CreateFrom(TierListState head)
        {
            PositionModel position = TargetPosition.CreateFrom(head);
            return new PositionChangeModel
            {
                UtcTime = DateTime.UtcNow,
                TargetPerson = position.GetPerson(head)!,
                TargetPosition = position,
                NewPosition = NewPosition.CreateFrom(head),
            };
        }
    }
}
