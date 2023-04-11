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

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeprops = null)
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeprops != null)
            {
                foreach (var includeProp in includeprops.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeprops = null)
        {

            IQueryable<T> query = _context.Set<T>().AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeprops != null)
            {
                foreach (var includeProp in includeprops.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.ToListAsync();
        }

        //public Task<T> GetFirstAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> filter, string? includeprops = null)
        {
            
             var dbset = _context.Set<T>().Where(filter);
            if (includeprops != null)
            {
                foreach (var includeProp in includeprops.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    dbset = dbset.Include(includeProp);
                }
            }
            return await dbset.FirstOrDefaultAsync();
        }

        

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }


        public bool Exists(int id)
        {
            var entity = Get(id);
            return entity != null;
        }

        private T Get(int id)
        {
            return _context.Set<T>().Find(id);
        }
    }
}
