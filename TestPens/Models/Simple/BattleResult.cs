namespace TestPens.Models.Simple
{
    public enum BattleResult
    {
        Unfinished,
        LeftWin,
        RightWin,
        Draw,
    }

    public static class BattleResultExtensions
    {
        public static (string, string)? GetColors(this BattleResult battleResult) => battleResult switch
        {
            BattleResult.LeftWin => ("#00d4ff", "#0080f5"),
            BattleResult.RightWin => ("#e64189", "#ff0f0f"),
            BattleResult.Draw => ("#b398ff", "#9200fa"),
            _ => null,
        };
    }
}
