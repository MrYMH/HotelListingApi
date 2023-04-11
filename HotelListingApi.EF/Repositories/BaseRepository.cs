//using HotelHostingApi.EF.Data;

using HotelHostingApi.EF.Data;
using HotelLisstingApi.Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HotelListingApi.EF.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context; 

        public BaseRepository(ApplicationDbContext context) 
        {
            _context = context;
        }


        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {

            return await _context.Set<T>().ToListAsync();
        }

        //public Task<T> GetFirstAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<T> GetFirstAsync(Expression<Func<T,bool>> cond, string[] includeprops)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(cond);
        }

        

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
