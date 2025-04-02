using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories
{
    public class GenericRepo<TEntity, Tkey> where TEntity : class
    {
        private readonly BlinkDbContext db;
        public GenericRepo(BlinkDbContext _db)
        { 
            db = _db;
        }
        public virtual async Task<List<TEntity>> GetAll()
        {
            return await db.Set<TEntity>().ToListAsync();
        }
        public virtual async Task<TEntity?> GetById(Tkey id)
        {
            return await db.Set<TEntity>().FindAsync(id);
        }
        public virtual void Add(TEntity entity)
        {
            db.Set<TEntity>().Add(entity);

        }
        public void Update(TEntity entity)
        {
            db.Entry(entity).State = EntityState.Modified;

        }
        public virtual async void Delete(Tkey id)
        {
            TEntity? t = await GetById(id);
            if (t != null)
            {
                var prop = t.GetType().GetProperty("IsDeleted");
                if (prop != null)
                {
                    prop.SetValue(t, true);
                    Update(t);

                }
            }
        }
        public async Task SaveChanges()
        {
           await db.SaveChangesAsync();
        }
    }
}
