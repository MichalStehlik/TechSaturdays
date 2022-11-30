using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using TechSaturdays.Data;
using TechSaturdays.Interfaces;

namespace TechSaturdays.Services
{
    public abstract class BasicRepository<TKey, TEntity> : IDisposable, IRepository<TKey, TEntity> where TEntity : class
    {
        internal readonly ApplicationDbContext _context;
        public DbSet<TEntity> DbSet { get; protected set; }
        private bool _disposed = false;

        public BasicRepository(ApplicationDbContext context)
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual TEntity? Read(TKey id)
        {
            return DbSet.Find(id);
        }

        public virtual ValueTask<TEntity?> ReadAsync(TKey id)
        {
            return DbSet.FindAsync(id);
        }

        public virtual void Create(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
        }

        public virtual void SetState(TEntity entity, EntityState state)
        {
            _context.Entry(entity).State = state;
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }

        public virtual Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public List<TEntity> List<TOrderBy>(
            Expression<Func<TEntity, bool>>? filter = null,
            Expression<Func<TEntity, TOrderBy>>? orderBy = null,
            bool? orderDescending = false,
            int page = 0,
            int pagesize = 0)
        {
            IQueryable<TEntity> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null && orderDescending != null)
            {
                if ((bool)orderDescending) query = query.OrderByDescending(orderBy); else query = query.OrderBy(orderBy);
            }
            if (orderBy != null && orderDescending != null)
            {
                if ((bool)orderDescending) query = query.OrderByDescending(orderBy); else query = query.OrderBy(orderBy);
            }
            if (pagesize != 0)
            {
                query = query.Skip(page).Take(pagesize);
            }
            return query.ToList();
        }
    }
}
