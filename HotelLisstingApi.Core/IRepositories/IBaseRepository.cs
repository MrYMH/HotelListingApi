using HotelLisstingApi.Core.Dtos;
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
        public void Update(T entity);
        public void Delete(T entity);
        public Task AddAsync(T entity);

        bool Exists(int id);


        //new implement
        Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>>? filter = null, string? includeprops = null);
        Task<TResult> GetFirstAsync<TResult>(Expression<Func<T, bool>> filter = null, string? includeprops = null);

        Task UpdateAsync<TSource>(int id, TSource source) where TSource : IBaseDto;
        public Task<TResult> AddAsync<TSource ,TResult>(TSource source);

        Task<List<TResult>> GetAllAsync2<TResult>();

    }
}
