using System.Runtime.InteropServices.ComTypes;

using TestPens.Models.Real;

namespace TestPens.Models.Dto
{
    public interface IDtoObject<T>
        where T : class
    {
        public T CreateFrom(TierListState head);
    }
}
