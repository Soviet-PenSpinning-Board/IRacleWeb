using System.Runtime.CompilerServices;

using TestPens.Extensions;
using TestPens.Models.Dto;
using TestPens.Models.Real;
using TestPens.Models.Simple;

namespace TestPens.Models.Real;

public class BattleModel : IModelObject<BattleDto>
{
    public BattledPersonModel Left { get; set; } = null!;
    public BattledPersonModel Right { get; set; } = null!;

    public BattleResult Result { get; set; } = BattleResult.Unfinished;
}

public class BattledPersonModel : IModelObject<BattledPersonDto>
{
    public Guid Guid { get; set; }
    public PersonModel MainModel { get; set; } = null!;
    public PositionModel PreBattlePosition { get; set; } = null!;
    public string? VideoUrl { get; set; }

    public (PersonModel, PositionModel) GetActual(TierListState head)
    {
        foreach (var group in head.TierList)
        {
            for (int i = 0; i < group.Value.Count; i++)
            {
                if (group.Value[i].Guid == Guid)
                {
                    return (group.Value[i].Copy(), new PositionModel
                    {
                        Tier = group.Key,
                        TierPosition = i,
                    });
                }
            }
        }

        return (null!, PositionModel.Unknown);
    }
}