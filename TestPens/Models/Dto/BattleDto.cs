using System.Runtime.CompilerServices;

using TestPens.Extensions;
using TestPens.Models.Real;
using TestPens.Models.Simple;

namespace TestPens.Models.Dto;

public class BattleDto : IDtoObject<BattleModel>
{
    public BattledPersonDto Left { get; set; } = null!;
    public BattledPersonDto Right { get; set; } = null!;

    public BattleResult Result { get; set; } = BattleResult.Unfinished;

    public BattleModel CreateFrom(TierListState head)
    {
        return new BattleModel
        {
            Left = Left.CreateFrom(head),
            Right = Right.CreateFrom(head),
            Result = Result,
        };
    }
}

public class BattledPersonDto : IDtoObject<BattledPersonModel>
{
    public Guid Guid { get; set; }
    public PersonModel? MainModel { get; set; } = null!;
    public PositionModel? PreBattlePosition { get; set; } = null!;
    public string? VideoUrl { get; set; }

    public BattledPersonModel CreateFrom(TierListState head)
    {
        BattledPersonModel toRet = new BattledPersonModel
        {
            Guid = Guid,
        };

        if (!string.IsNullOrWhiteSpace(VideoUrl))
        {
            toRet.VideoUrl = VideoUrl.TransformToIframeUrl();
        }

        foreach (var group in head.TierList)
        {
            for (int i = 0; i < group.Value.Count; i++)
            {
                if (group.Value[i].Guid == Guid)
                {
                    toRet.MainModel = group.Value[i].Copy();
                    toRet.PreBattlePosition = new PositionModel
                    {
                        Tier = group.Key,
                        TierPosition = i,
                    };

                    if (string.IsNullOrWhiteSpace(toRet.VideoUrl))
                    {
                        toRet.VideoUrl = toRet.MainModel.VideoLink;
                    }

                    return toRet;
                }
            }
        }

        toRet.MainModel = null!;
        toRet.PreBattlePosition = PositionModel.Unknown;

        return toRet;
    }
}