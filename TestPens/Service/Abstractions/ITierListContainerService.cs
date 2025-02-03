using TestPens.Models;
using TestPens.Models.Dto;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Real;
using TestPens.Models.Real.Changes;

namespace TestPens.Service.Abstractions;

public interface ITierListContainerService
{
    public TierListState GetHead();
    public void Update(TierListState newState);
}
