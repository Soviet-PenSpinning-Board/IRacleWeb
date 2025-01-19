namespace TestPens.Models.Simple;

public class BattleModel
{
    public BattleModel()
    {
        UtcTime = DateTime.UtcNow;
    }

    public BattleModel(BattledPersonModel left, BattledPersonModel right)
        : this()
    {
        Left = left;
        Right = right;
    }

    public BattledPersonModel Left { get; set; } = null!;
    public BattledPersonModel Right { get; set; } = null!;

    public DateTime UtcTime { get; set; }

    public void Initialize(TierListState tierListState)
    {
        (Left.MainModel, Left.PreBattlePosition) = Left.GetActualProperties(tierListState);
        (Right.MainModel, Right.PreBattlePosition) = Right.GetActualProperties(tierListState);
    }

    public BattleResult Result { get; set; } = BattleResult.Unfinished;
}

public class BattledPersonModel
{
    public Guid Guid { get; set; }
    public PersonModel? MainModel { get; set; } = null!;
    public ShortPositionModel? PreBattlePosition { get; set; } = null!;
    public string? VideoUrl { get; set; }

    public (PersonModel?, ShortPositionModel) GetActualProperties(TierListState tierListState)
    {
        foreach (var group in tierListState.TierList)
        {
            for (int i = 0; i < group.Value.Count; i++)
            {
                if (group.Value[i].Guid == Guid)
                {
                    return (group.Value[i], new ShortPositionModel { Tier = group.Key, TierPosition = i });
                }
            }
        }

        return (MainModel, ShortPositionModel.Unknown);
    }
}

public enum BattleResult
{
    Unfinished,
    LeftWin,
    RightWin,
    Draw,
}
