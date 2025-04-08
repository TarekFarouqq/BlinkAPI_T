using Blink_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Blink_API.Repositories
{
    public class GenericRepo<TEntity, Tkey> where TEntity : class
    {
        private readonly BlinkDbContext db;
        public GenericRepo(BlinkDbContext _db)
        { 
            db = _db;
        }
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await db.Set<TEntity>().ToListAsync();
        }
        public virtual async Task<TEntity?> GetById(Tkey id)
        {
            return await db.Set<TEntity>().FindAsync(id);
        }
        public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await db.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
        public virtual void Add(TEntity entity)
        {
            db.Set<TEntity>().Add(entity);

        }
        public virtual void Update(TEntity entity)
        {
             db.Update(entity);
        }
        public virtual void Delete(TEntity entity)
        {
            db.Remove(entity);
        }
        public async Task SaveChanges()
        {
           await db.SaveChangesAsync();
        }
    }
}
