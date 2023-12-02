using System.Linq.Expressions;


namespace WebTimetable.Application.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> selector) 
        {
            return _context.Set<T>().Where(selector).AsQueryable();
        }
        
        public async Task<T?> FindAsync(CancellationToken token, params object?[]? objects)
        {
            return await _context.Set<T>().FindAsync(objects, token);
        }

        public async Task AddAsync(T newEntity, CancellationToken token)
        {
            await _context.Set<T>().AddAsync(newEntity, token);
        }

        public async Task AddRangeAsync(IEnumerable<T> newEntities, CancellationToken token)
        {
            await _context.Set<T>().AddRangeAsync(newEntities, token);
        }
        
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public async Task SaveChangesAsync(CancellationToken token)
        {
            await _context.SaveChangesAsync(token);
        }
    }
}