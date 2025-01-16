using TestPens.Models;
using TestPens.Models.Abstractions;

namespace TestPens.Service.Abstractions;

public interface IPersonContainerService
{
    public TierListState GetHead();

    public IReadOnlyCollection<BaseChange> GetAllChanges();

    public void AddChange(BaseChange change);
}
