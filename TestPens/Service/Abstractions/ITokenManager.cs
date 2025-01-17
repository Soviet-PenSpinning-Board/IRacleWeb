

namespace TestPens.Service.Abstractions
{
    public interface ITokenManager
    {
        public Permissions CheckToken(string token);
    }

    [Flags]
    public enum Permissions
    {
        None = 0,
        StartBattles = 1,
        EndBattles = 2,
        ChangeProperties = 4,
        NewMember = 8,
        DeleteMember = 16,
        ChangePositions = 32,
        GlobalChanges = 64,

        Battles = StartBattles | EndBattles,
        MembersEdit = ChangeProperties | NewMember | DeleteMember,
        EditAll = ChangePositions | MembersEdit,
        All = Battles | EditAll | GlobalChanges,
    }
}
