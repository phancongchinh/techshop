using System.Linq.Expressions;

namespace Techshop.Repository;

public interface IRepository<T> where T : class
{
    IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "");

    T? GetById(object id);

    void Insert(T entity);

    void Update(T entityToUpdate);

    void Delete(object id);

    void Delete(T entityToDelete);
}