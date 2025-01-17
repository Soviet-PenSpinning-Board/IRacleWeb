namespace TestPens.Models.Simple
{
    public enum Tier
    {
        SPlus,
        S,
        A,
        B,
        C,
        D,
        E
    }

    public static class TierExtensions
    {
        public static string GetName(this Tier tier) => tier switch
        {
            Tier.SPlus => "S+",
            _ => tier.ToString(),
        };

        public static (string, string) GetColors(this Tier tier) => tier switch
        {
            Tier.SPlus => ("#979797", "#595959"),
            Tier.S => ("#f81010", "#fc6363"),
            Tier.A => ("#f010f8", "#ff008d"),
            Tier.B => ("#00c5ff", "#004bff"),
            Tier.C => ("#d1b900", "#ff8700"),
            Tier.D => ("#e6ff00", "#b1ff00"),
            Tier.E => ("#00ff7a", "#00ff07"),
            _ => ("#ffffff", "#000000")
        };
    }
}
