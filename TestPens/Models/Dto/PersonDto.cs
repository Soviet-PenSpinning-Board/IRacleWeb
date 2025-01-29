using TestPens.Extensions;
using TestPens.Models.Real;

namespace TestPens.Models.Dto
{
    public class PersonDto : IDtoObject<PersonModel>
    {
        public Guid Guid { get; set; }

        public string Nickname { get; set; } = null!;

        public bool InDrop { get; set; }

        public string VideoLink { get; set; } = null!;

        public string AvatarUrl { get; set; } = null!;

        public PersonModel CreateFrom(TierListState head)
        {
            return new PersonModel()
            {
                Guid = Guid,
                Nickname = Nickname.Trim(),
                InDrop = InDrop,
                VideoLink = VideoLink.TransformToIframeUrl(),
                AvatarUrl = AvatarUrl,
            };
        }
    }
}
