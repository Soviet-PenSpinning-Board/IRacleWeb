using TestPens.Models.Dto;
using TestPens.Models.Real;
using TestPens.Models.Real.Changes;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Dto.Changes
{
    public class ChangePositionDto : ChangeBaseDto
    {
        public override ChangeType Type { get; set; } = ChangeType.ChangePosition;

        public PositionDto NewPosition { get; set; } = null!;

        public override ChangePositionModel CreateFrom(TierListState head)
        {
            PositionModel position = TargetPosition.CreateFrom(head);
            return new ChangePositionModel
            {
                UtcTime = DateTime.UtcNow,
                TargetPerson = position.GetPerson(head)!.Copy(),
                TargetPosition = position,
                NewPosition = NewPosition.CreateFrom(head),
            };
        }
    }
}
