using TestPens.Extensions;
using TestPens.Models.Real;
using TestPens.Models.Simple;

namespace TestPens.Models.Dto
{
    public class PositionDto : IDtoObject<PositionModel>
    {
        public Tier Tier { get; set; }

        public int TierPosition { get; set; }

        public PositionModel CreateFrom(TierListState head)
        {
            return new PositionModel
            {
                Tier = Tier,
                TierPosition = TierPosition,
            };
        }
    }
}
