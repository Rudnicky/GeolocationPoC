using GeolocationPoC.Core.Domain.Db;
using GeolocationPoC.Core.Interfaces.DatabaseAccessLayer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeolocationPoC.Persistence.Repositories.DatabaseAccessLayer
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        protected readonly GeolocationDbContext Context;

        public Repository(GeolocationDbContext context)
        {
            Context = context;
        }

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
        }

        public async Task<T> Get(int Id)
        {
            return await Context.Set<T>().SingleOrDefaultAsync(x => x.Id == Id).ConfigureAwait(false);
        }

        public async Task<List<T>> GetAll()
        {
            return await Context.Set<T>().AsQueryable().ToListAsync().ConfigureAwait(false);
        }

        public void Update(T entity)
        {
            Context.Set<T>().Update(entity);
            Context.SaveChanges();
        }
    }
}
