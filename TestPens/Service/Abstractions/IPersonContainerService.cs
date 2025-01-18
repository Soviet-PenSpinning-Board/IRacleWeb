using TestPens.Models;
using TestPens.Models.Abstractions;

namespace TestPens.Service.Abstractions;

public interface IPersonContainerService
{
    public TierListState GetHead();

    public IReadOnlyList<BaseChange> GetAllChanges();

    public void AddChanges(IEnumerable<BaseChange> changes);

    public void RevertLast(int count);

    public void RevertAllAfter(DateTime utsTime);
}
