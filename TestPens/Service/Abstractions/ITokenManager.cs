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
        GlobalMember = 8,
        ChangePositions = 16,
        GlobalChanges = 32,

        Battles = StartBattles | EndBattles,
        MembersEdit = ChangeProperties | GlobalMember,
        EditAll = ChangePositions | MembersEdit,
        All = Battles | EditAll | GlobalChanges,
    }
}
