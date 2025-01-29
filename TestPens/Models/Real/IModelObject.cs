using TestPens.Models.Dto;

namespace TestPens.Models.Real
{
    public interface IModelObject<T>
        where T : class
    {
        public T ToForm();
    }
}
