using TestPens.Models.Real;
using TestPens.Models.Real.Changes;
using TestPens.Models.Shared;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Dto.Changes
{
    public class ChangePositionDto : ChangeBaseDto
    {
        public override ChangeType Type { get; set; } = ChangeType.ChangePosition;

        public PositionModel NewPosition { get; set; } = null!;

        public override ChangePositionModel CreateFrom(TierListState head)
        {
            return new ChangePositionModel
            {
                UtcTime = DateTime.UtcNow,
                TargetPerson = TargetPosition.GetPerson(head)!.Copy(),
                TargetPosition = TargetPosition,
                NewPosition = NewPosition,
            };
        }
    }
}
