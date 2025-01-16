
using TestPens.Models;
using TestPens.Models.Abstractions;
using TestPens.Service.Abstractions;

namespace TestPens.Service;

public class JsonPersonContainerService : IPersonContainerService
{
    public IReadOnlyCollection<BaseChange> GetAllChanges()
    {
        throw new NotImplementedException();
    }

    public TierListState GetHead()
    {
        throw new NotImplementedException();
    }
}
