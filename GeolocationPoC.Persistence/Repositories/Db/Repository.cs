using GeolocationPoC.Core.Domain.Db;
using GeolocationPoC.Core.Interfaces.Db;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeolocationPoC.Persistence.Repositories.Db
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly GeolocationDbContext _context;

        public Repository(GeolocationDbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public T Get(int Id)
        {
            return _context.Set<T>().Find(Id);
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().AsQueryable().ToListAsync();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }
    }
}
