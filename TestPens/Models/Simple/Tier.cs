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

        public static (string, string) GetColorsDark(this Tier tier) => tier switch
        {
            Tier.SPlus => ("#5e5e5e", "#303030"),
            Tier.S => ("#8c0101", "#451a1a"),
            Tier.A => ("#b031b5", "#910050"),
            Tier.B => ("#1d7c98", "#002681"),
            Tier.C => ("#9c8a05", "#965000"),
            Tier.D => ("#9aa813", "#4e7000"),
            Tier.E => ("#2eab6a", "#007103"),
            _ => ("#000000", "#ffffff")
        };
    }
}
