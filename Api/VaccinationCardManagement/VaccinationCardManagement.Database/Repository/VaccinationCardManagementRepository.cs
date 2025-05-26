using System.Linq.Expressions;
using VaccinationCardManagement.Database.Context;
using VaccinationCardManagement.Domain.Adapter;

namespace VaccinationCardManagement.Database.Repository;

public class VaccinationCardManagementRepository : IVaccinationCardManagementRepository
{
    private readonly VaccinationCardManagementContext _context;

    public VaccinationCardManagementRepository(VaccinationCardManagementContext context)
    {
        _context = context;
    }

    public async Task<T> FindOne<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        T item = null;
        var query = _context.Set<T>().FirstOrDefault(predicate);
        item = query;
        return item;
    }

    public async Task<IEnumerable<T>> Find<T>(Expression<Func<T, bool>> predicate = null) where T : class
    {
        var result = Enumerable.Empty<T>();
        IEnumerable<T> query = null;
        if (predicate != null)
        {
            query = _context.Set<T>().Where(predicate).ToList();
        }
        else
        {
            query = _context.Set<T>().ToList();
        }
        result = query;

        return result;
    }

    public async Task<T> Add<T>(T request) where T : class
    {
        await _context.Set<T>().AddAsync(request);
        await _context.Commit(CancellationToken.None);

        return request;
    }

    public async Task<T> Update<T>(T request) where T : class
    {
        _context.Set<T>().Update(request);
        await _context.Commit(CancellationToken.None);

        return request;
    }

    public async Task<List<T>> UpdateRange<T>(List<T> entities) where T : class
    {
        var baseEntities = entities as T[] ?? entities.ToArray();
        _context.Set<T>().UpdateRange(baseEntities);
        await _context.Commit(CancellationToken.None);

        return baseEntities.ToList();
    }

    public async Task AddRange<T>(IEnumerable<T> entities) where T : class
    {
        var baseEntities = entities as T[] ?? entities.ToArray();
        await _context.Set<T>().AddRangeAsync(baseEntities);
        await _context.Commit(CancellationToken.None);
    }

    public async Task Delete<T>(T entity) where T : class
    {
        _context.Set<T>().Remove(entity);
        await _context.Commit(CancellationToken.None);
    }

    public async Task DeleteMany<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        var entities = await Find(predicate);
        _context.Set<T>().RemoveRange(entities);
        await _context.Commit(CancellationToken.None);
    }
}