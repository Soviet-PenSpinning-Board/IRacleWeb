namespace TestPens.Models.Simple
{
    public class PersonModel
    {
        public string Nickname { get; set; } = null!;

        public bool InDrop { get; set; }

        public string? VideoLink { get; set; }

        public string AvatarUrl { get; set; } = null!;
    }

    public class ShortPositionModule
    {
        public Tier Tier { get; set; }

        public int TierPosition { get; set; }
    }
}
