using System.Diagnostics.CodeAnalysis;

using TestPens.Extensions;
using TestPens.Models.Dto;
using TestPens.Models.Real;

namespace TestPens.Models.Shared
{
    public class PersonModel
    {
        public Guid Guid { get; set; }

        public string Nickname { get; set; } = $"Случайное имя";

        public bool InDrop { get; set; }

        public string VideoLink { get; set; } = string.Empty;

        public string AvatarUrl { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public PersonModel ConvertProps()
        {
            string avatarUrl = string.IsNullOrWhiteSpace(AvatarUrl) ? "/avatars/default.png" : AvatarUrl;
            return new PersonModel()
            {
                Guid = Guid,
                Nickname = Nickname.Trim(),
                InDrop = InDrop,
                VideoLink = VideoLink.Trim().TransformToIframeUrl(),
                AvatarUrl = avatarUrl.Trim(),
                Description = Description.Trim(),
            };
        }

        public PositionModel? GetPosition(TierListState head)
        {
            foreach (var tierGroup in head.TierList)
            {
                for (int i = 0; i < tierGroup.Value.Count; i++)
                {
                    if (tierGroup.Value[i].Guid == Guid)
                    {
                        return new PositionModel
                        {
                            Tier = tierGroup.Key,
                            TierPosition = i,
                        };
                    }
                }
            }

            return null;
        }

        public PersonModel Copy()
        {
            return new PersonModel()
            {
                Guid = Guid,
                Nickname = Nickname,
                InDrop = InDrop,
                VideoLink = VideoLink,
                AvatarUrl = AvatarUrl,
                Description = Description,
            };
        }

        public static bool operator ==(PersonModel? left, PersonModel? right)
        {
            if (left is null || right is null)
            {
                return ReferenceEquals(left, right);
            }

            return left.Guid == right.Guid &&
                left.Nickname == right.Nickname &&
                left.InDrop == right.InDrop &&
                left.VideoLink == right.VideoLink &&
                left.AvatarUrl == right.AvatarUrl &&
                left.Description == right.Description;
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
            return HashCode.Combine(Guid, Nickname, InDrop, VideoLink, AvatarUrl, Description);
        }
    }
}
