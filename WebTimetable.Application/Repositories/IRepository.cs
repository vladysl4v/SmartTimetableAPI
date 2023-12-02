using System.Linq.Expressions;


namespace WebTimetable.Application.Repositories
{
    public interface IRepository<T> where T : class
    {
        public IQueryable<T> GetAll();
        public IQueryable<T> Where(Expression<Func<T, bool>> selector);
        public Task<T?> FindAsync(CancellationToken token, params object?[]? objects);
        public Task AddAsync(T newEntity, CancellationToken token);
        public Task AddRangeAsync(IEnumerable<T> newEntities, CancellationToken token);
        public void Remove(T entity);
        public void RemoveRange(IEnumerable<T> entities);
        public void Update(T entity);
        public Task SaveChangesAsync(CancellationToken token);
    }
}