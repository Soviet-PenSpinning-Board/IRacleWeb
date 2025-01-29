using TestPens.Models.Dto;
using TestPens.Models.Simple;

namespace TestPens.Models.Real
{
    public class PositionModel : IModelObject<PositionDto>
    {
        public static PositionModel Unknown { get; } = new PositionModel
        {
            Tier = Tier.E,
            TierPosition = -1,
        };

        public Tier Tier { get; set; }

        public int TierPosition { get; set; }

        public PersonModel? GetPerson(TierListState head)
        {
            if (TierPosition >= 0 && head.TierList.TryGetValue(Tier, out List<PersonModel>? list) && TierPosition < list.Count)
            {
                return list[TierPosition];
            }

            return null;
        }

        public PositionDto ToForm()
        {
            return new PositionDto
            {
                Tier = Tier,
                TierPosition = TierPosition
            };
        }

        public static bool operator ==(PositionModel? left, PositionModel? right)
        {
            if (left is null || right is null)
            {
                return ReferenceEquals(left, left);
            }
            return left.Tier == right.Tier && left.TierPosition == right.TierPosition;
        }

        public static bool operator !=(PositionModel? left, PositionModel? right)
        {
            return !(left == right);
        }

        public static bool operator >(PositionModel left, PositionModel right)
        {
            return left.Tier > right.Tier || (left.Tier == right.Tier && left.TierPosition > right.TierPosition);
        }

        public static bool operator <(PositionModel left, PositionModel right)
        {
            return right > left;
        }

        public static bool operator <=(PositionModel left, PositionModel right)
        {
            return left < right || left == right;
        }

        public static bool operator >=(PositionModel left, PositionModel right)
        {
            return left > right || left == right;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is not PositionModel typeObj)
            {
                return false;
            }

            return typeObj == this;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tier, TierPosition);
        }
    }
}
