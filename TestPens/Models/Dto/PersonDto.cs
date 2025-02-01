using TestPens.Extensions;
using TestPens.Models.Real;

namespace TestPens.Models.Dto
{
    public class PersonDto : IDtoObject<PersonModel>
    {
        public Guid Guid { get; set; }

        public string Nickname { get; set; } = $"Случайное имя";

        public bool InDrop { get; set; }

        public string VideoLink { get; set; } = string.Empty;

        public string AvatarUrl { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public PersonModel CreateFrom(TierListState head)
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
    }
}
