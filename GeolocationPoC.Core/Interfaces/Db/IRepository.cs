using GeolocationPoC.Core.Domain.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeolocationPoC.Core.Interfaces.Db
{
    public interface IRepository<T> where T : EntityBase
    {
        T Get(int Id);
        Task<List<T>> GetAll();
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
