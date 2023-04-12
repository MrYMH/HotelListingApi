using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HotelLisstingApi.Core.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        public IEnumerable<T> GetAll();
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> GetFirstAsync(Expression<Func<T, bool>> cond, string[] includeprops = null);
        public Task UpdateAsync(T entity);
        public void Delete(T entity);
        public Task AddAsync(T entity);
        public Task SaveAsync();
        public void Save();

    }
}
