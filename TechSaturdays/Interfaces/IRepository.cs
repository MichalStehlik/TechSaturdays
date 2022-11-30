using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TechSaturdays.Interfaces
{
    public interface IRepository<TKey, TEntity> where TEntity : class
    {
        DbSet<TEntity> DbSet { get; }
        List<TEntity> List<TOrderBy>(
            Expression<Func<TEntity, bool>>? filter = null,
            Expression<Func<TEntity, TOrderBy>>? orderBy = null,
            bool? orderDescending = false,
            int page = 0,
            int pagesize = 0
            );
        TEntity? Read(TKey id);
        ValueTask<TEntity?> ReadAsync(TKey key);
        void Delete(TEntity data);
        void Update(TEntity value);
        void SetState(TEntity entity, EntityState state);
        void Create(TEntity data);
        void Save();
        Task SaveAsync();
    }
}
