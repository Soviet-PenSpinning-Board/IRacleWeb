using TestPens.Models;
using TestPens.Models.Abstractions;

namespace TestPens.Service.Abstractions;

public interface IPersonContainerService
{
    public TierListState GetHead();

    public IEnumerable<BaseChange> GetAllChanges(int offset = 0, int limit = int.MaxValue, DateTime? afterTime = null);

    public void AddChanges(IEnumerable<BaseChange> changes);

    public void RevertLast(int count);

    public void RevertAllAfter(DateTime utsTime);

    public TierListState RevertLastNode(int count);

    public TierListState RevertAllAfterNode(DateTime utsTime);
}
