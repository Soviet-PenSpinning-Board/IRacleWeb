using TestPens.Models;
using TestPens.Models.Dto;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Real.Changes;

namespace TestPens.Service.Abstractions;

public interface IPersonContainerService
{
    public TierListState GetHead();

    public IEnumerable<ChangeBaseModel> GetAllChanges(int offset = 0, int limit = int.MaxValue, DateTime? afterTime = null);

    public void AddChanges(IReadOnlyCollection<ChangeBaseDto> changes);

    public void RevertLast(int count);

    public void RevertAllAfter(DateTime utsTime);

    public TierListState RevertLastNode(int count);

    public TierListState RevertAllAfterNode(DateTime utsTime);
}
