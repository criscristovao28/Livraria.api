using livraria.api.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace livraria.api.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected LibraryContext Context;
        internal DbSet<T> db;

        public BaseRepository(LibraryContext context)
        {
            Context = context;
            db = context.Set<T>();
        }

        public virtual async Task<T> GetById(Guid Id)
        {
            var item = await db.FindAsync(Id);
            if (item == null) throw new Exception("Não encontrado");
            return item;
        }

        public virtual async Task<T> Add(T entity)
        {
            await db.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> Update(T entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> Delete(Guid id)
        {
            var item = await GetById(id);
            db.Remove(item);
            await Context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<IEnumerable<T>> Get()
        {
            return await db.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> Get(int size, int page)
        {
            return await db.Skip(page * size).Take(size).ToListAsync();
        }
    }
}
