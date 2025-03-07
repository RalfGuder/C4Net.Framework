using System.Linq;

namespace C4Net.Framework.Data.Expressions
{
    public interface IQueryableObject : IOrderedQueryable
    {
        IDataContext Adapter { get; }
    }

    public interface IQueryableObject<T> : IQueryableObject, IOrderedQueryable<T>
    {
    }

}
