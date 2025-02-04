namespace TestPens.Models.Simple
{
    public enum TierListType
    {
        Main,
        Short
    }

    public static class TierListTypeExtensions
    {
        public static string GetView(this TierListType type) => type switch
        {
            TierListType.Short => "TierList/_ShortTierListPartial",
            _ => "TierList/_MainTierListPartial"
        };
}
}
