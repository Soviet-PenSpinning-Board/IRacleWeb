namespace TestPens.Models.Simple
{
    public class PersonModel
    {
        public PersonModel()
        {
            Guid = Guid.NewGuid();
        }

        public Guid Guid { get; set; }

        public string Nickname { get; set; } = null!;

        public bool InDrop { get; set; }

        public string? VideoLink { get; set; }

        public string? AvatarUrl { get; set; }

        public static bool operator ==(PersonModel? left, PersonModel? right)
        {
            if (left is null || right is null)
            {
                return ReferenceEquals(left, left);
            }
            return left.Guid == right.Guid && 
                left.Nickname == right.Nickname &&
                left.InDrop == right.InDrop &&
                left.VideoLink == right.VideoLink &&
                left.AvatarUrl == right.AvatarUrl;
        }

        public static bool operator !=(PersonModel? left, PersonModel? right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is not PersonModel typeObj)
            {
                return false;
            }

            return typeObj == this;
        }

        public override int GetHashCode()
        {
            return (Guid, Nickname, InDrop, VideoLink, AvatarUrl).GetHashCode();
        }
    }

    public class ShortPositionModel
    {
        public Tier Tier { get; set; }

        public int TierPosition { get; set; }

        public static ShortPositionModel Unknown { get; } = new ShortPositionModel { Tier = Tier.E, TierPosition = -1 };

        public static bool operator ==(ShortPositionModel left, ShortPositionModel right)
        {
            if (left is null || right is null)
            {
                return ReferenceEquals(left, left);
            }
            return left.Tier == right.Tier && left.TierPosition == right.TierPosition;
        }

        public static bool operator !=(ShortPositionModel left, ShortPositionModel right)
        {
            return !(left == right);
        }

        public static bool operator >(ShortPositionModel left, ShortPositionModel right)
        {
            return left.Tier > right.Tier || (left.Tier == right.Tier && left.TierPosition > right.TierPosition);
        }

        public static bool operator <(ShortPositionModel left, ShortPositionModel right)
        {
            return right > left;
        }

        public static bool operator <=(ShortPositionModel left, ShortPositionModel right)
        {
            return left < right || left == right;
        }

        public static bool operator >=(ShortPositionModel left, ShortPositionModel right)
        {
            return left > right || left == right;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is not ShortPositionModel typeObj)
            {
                return false;
            }

            return typeObj == this;
        }

        public override int GetHashCode()
        {
            return (Tier, TierPosition).GetHashCode();
        }
    }
}
