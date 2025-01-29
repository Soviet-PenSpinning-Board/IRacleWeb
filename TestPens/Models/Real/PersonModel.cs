using System;

using TestPens.Models.Dto;

namespace TestPens.Models.Real
{
    public class PersonModel : IModelObject<PersonDto>
    {
        public Guid Guid { get; set; }

        public string Nickname { get; set; } = null!;

        public bool InDrop { get; set; }

        public string VideoLink { get; set; } = null!;

        public string AvatarUrl { get; set; } = null!;

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

        public PersonDto ToForm()
        {
            return new PersonDto
            {
                Guid = Guid,
                Nickname = Nickname,
                InDrop = InDrop,
                VideoLink = VideoLink,
                AvatarUrl = AvatarUrl,
            };
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
            };
        }

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
            return HashCode.Combine(Guid, Nickname, InDrop, VideoLink, AvatarUrl);
        }
    }
}
