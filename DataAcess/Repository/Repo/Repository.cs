using DataAcess.DbContexts;
using DataAcess.Repos.IRepos;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAcess.Repos
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _db;
        protected readonly DbSet<T> DbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            DbSet = _db.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public Task DeleteAsync(T entity)
        {
            DbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<T?> GetAsync(
            Expression<Func<T, bool>> filter = null,
            string? includes = null)
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(includes))
            {
                foreach (var include in includes.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include.Trim());
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            string? includes = null,
            int pageSize = 0,
            int pageNumber = 1)
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(includes))
            {
                foreach (var include in includes.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include.Trim());
                }
            }

            if (pageSize > 0)
            {
                pageSize = Math.Min(pageSize, 100);
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }

            return await query.AsNoTracking().ToListAsync();
        }
    }
}
