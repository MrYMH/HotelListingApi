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
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeprops = null);
        public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter, string? includeprops = null);
        public Task<T> GetFirstAsync(Expression<Func<T, bool>> filter = null, string? includeprops = null);
        public Task UpdateAsync(T entity);
        public void Delete(T entity);
        public Task AddAsync(T entity);
        

    }
}
