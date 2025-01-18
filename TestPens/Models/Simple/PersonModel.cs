namespace TestPens.Models.Simple
{
    public class PersonModel
    {
        public string Nickname { get; set; } = null!;

        public bool InDrop { get; set; }

        public string? VideoLink { get; set; }

        public string? AvatarUrl { get; set; }

        public string ToSimpleJson()
        {
            return $$"""
                {
                    Nickname: "{{Nickname}}",
                    InDrop: "{{InDrop}}",
                    VideoLink: "{{VideoLink}}",
                    AvatarUrl: "{{AvatarUrl}}",
                }
                """;
        }
    }

    public class ShortPositionModel
    {
        public Tier Tier { get; set; }

        public int TierPosition { get; set; }
    }
}
