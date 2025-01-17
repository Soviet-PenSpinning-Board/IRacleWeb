using TestPens.Models;
using TestPens.Models.Abstractions;

namespace TestPens.Service.Abstractions;

public interface IPersonContainerService
{
    public TierListState GetHead();

    public IReadOnlyList<BaseChange> GetAllChanges();

    public void AddChange(BaseChange change);

    public void RevertLast(int count);

    public void RevertAllAfter(DateTime utsTime);
}
