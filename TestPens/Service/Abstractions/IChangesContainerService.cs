using TestPens.Models.Dto.Changes;
using TestPens.Models.Real.Changes;
using TestPens.Models;

namespace TestPens.Service.Abstractions;

public interface IChangesContainerService
{
    public Task<IEnumerable<ChangeBaseModel>> GetAllChanges(int offset = 0, int limit = int.MaxValue);

    public Task<int> GetCount();

    public Task AddChanges(IReadOnlyCollection<ChangeBaseDto> changes);

    public Task RevertLast(int count);

    public Task RevertAllAfter(DateTime utsTime);

    public Task<TierListState> RevertLastNode(int count);

    public Task<TierListState> RevertAllAfterNode(DateTime utsTime);
}
