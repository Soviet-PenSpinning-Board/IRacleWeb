using TestPens.Models.Dto.Changes;
using TestPens.Models.Real.Changes;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Simple
{
    public enum ChangeType
    {
        None,
        ChangePosition,
        GlobalPerson,
        PersonProperties
    }

    public static class ChangeTypeExtensions
    {
        public static Permissions GetPermissions(this ChangeType type) => type switch
        {
            ChangeType.None => Permissions.None,
            ChangeType.ChangePosition => Permissions.ChangePositions,
            ChangeType.GlobalPerson => Permissions.GlobalMember,
            ChangeType.PersonProperties => Permissions.ChangeProperties,
            _ => Permissions.None,
        };

        public static Type? GetDtoType(this ChangeType type) => type switch
        {
            ChangeType.None => typeof(ChangeNoneDto),
            ChangeType.ChangePosition => typeof(ChangePositionDto),
            ChangeType.GlobalPerson => typeof(ChangeGlobalPersonDto),
            ChangeType.PersonProperties => typeof(ChangePersonPropertiesDto),
            _ => null,
        };

        public static Type? GetModelType(this ChangeType type) => type switch
        {
            ChangeType.None => typeof(ChangeNoneModel),
            ChangeType.ChangePosition => typeof(ChangePositionModel),
            ChangeType.GlobalPerson => typeof(ChangeGlobalPersonModel),
            ChangeType.PersonProperties => typeof(ChangePersonPropertiesModel),
            _ => null,
        };
    }
}
