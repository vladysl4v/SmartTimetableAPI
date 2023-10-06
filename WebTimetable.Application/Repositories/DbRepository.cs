using System.Linq.Expressions;



namespace WebTimetable.Application.Repositories
{
    public class DbRepository : IDbRepository
    {
        private readonly DataContext _context;

        public DbRepository(DataContext context)
        {
            _context = context;
        }

        public IQueryable<T> Get<T>() where T : class
        {
            return _context.Set<T>().AsQueryable();
        }

        public IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class
        {
            return _context.Set<T>().Where(selector).AsQueryable();
        }

        public async Task Add<T>(T newEntity) where T : class
        {
            await _context.Set<T>().AddAsync(newEntity);
        }

        public async Task AddRange<T>(IEnumerable<T> newEntities) where T : class
        {
            await _context.Set<T>().AddRangeAsync(newEntities);
        }

        public void Remove<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }

        public void RemoveRange<T>(IEnumerable<T> entities) where T : class
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Set<T>().Update(entity);
        }

        public void UpdateRange<T>(IEnumerable<T> entities) where T : class
        {
            _context.Set<T>().UpdateRange(entities);
        }

        public async Task SaveChangesAsync(CancellationToken token)
        {
            await _context.SaveChangesAsync(token);
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return _context.Set<T>().AsQueryable();
        }
    }
}
