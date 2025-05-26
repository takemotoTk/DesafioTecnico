using System.Linq.Expressions;

namespace VaccinationCardManagement.Domain.Adapter;

public interface IGenericRepositoryBase
{
    Task<IEnumerable<T>> Find<T>(Expression<Func<T, bool>> predicate = null) where T : class;
    Task<T> FindOne<T>(Expression<Func<T, bool>> predicate) where T : class;
    Task<T> Add<T>(T request) where T : class;
    Task AddRange<T>(IEnumerable<T> entities) where T : class;
    Task<T> Update<T>(T request) where T : class;
    Task<List<T>> UpdateRange<T>(List<T> entities) where T : class;
    Task Delete<T>(T entity) where T : class;
    Task DeleteMany<T>(Expression<Func<T, bool>> predicate) where T : class;
}