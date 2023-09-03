using System.Linq.Expressions;


namespace WebTimetableApi.DAL.Repositories
{
    public interface IDbRepository
    {
        IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class;
        IQueryable<T> Get<T>() where T : class;
        IQueryable<T> GetAll<T>() where T : class;

        Task Add<T>(T newEntity) where T : class;
        Task AddRange<T>(IEnumerable<T> newEntities) where T : class;

        void Remove<T>(T entity) where T : class;
        void RemoveRange<T>(IEnumerable<T> entities) where T : class;

        void Update<T>(T entity) where T : class;
        void UpdateRange<T>(IEnumerable<T> entities) where T : class;

        Task SaveChangesAsync();
    }
}
