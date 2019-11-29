using GeolocationPoC.Core.Domain.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeolocationPoC.Core.Interfaces.DatabaseAccessLayer
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> Get(int Id);
        Task<List<T>> GetAll();
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
