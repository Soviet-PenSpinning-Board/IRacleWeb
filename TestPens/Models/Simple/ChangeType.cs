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
    }
}
