//using HotelHostingApi.EF.Data;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelHostingApi.EF.Data;
using HotelLisstingApi.Core.Dtos;
using HotelLisstingApi.Core.IRepositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HotelListingApi.EF.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BaseRepository(ApplicationDbContext context , IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
        {
            var totalSize = await _context.Set<T>().CountAsync();
            var items = await _context.Set<T>()
                .Skip(queryParameters.StartIndex)
                .Take(queryParameters.PageSize)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResult<TResult>
            {
                Items = items,
                PageNumber = queryParameters.PageNumber,
                RecordNumber = queryParameters.PageSize,
                TotalCount = totalSize
            };

        }


        //new implement
        public async Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>> filter = null, string? includeprops = null)
        {
            var query = _context.Set<T>().AsQueryable();
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
            var x = query.ProjectTo<TResult>(_mapper.ConfigurationProvider).ToList();
            return x ;
        }
        public async Task<TResult> AddAsync<TSource, TResult>(TSource source)
        {
            var entity = _mapper.Map<T>(source);

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TResult>(entity);
        }

        public async Task<TResult> GetFirstAsync<TResult>(Expression<Func<T, bool>> filter = null, string includeprops = null)
        {
            var dbset = _context.Set<T>().Where(filter);
            if (includeprops != null)
            {
                foreach (var includeProp in includeprops.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    dbset = dbset.Include(includeProp);
                }
            }
            var query = await dbset.FirstOrDefaultAsync();
            return _mapper.Map<TResult>(query);
        }

        public async Task UpdateAsync<TSource>(int id, TSource source) where TSource : IBaseDto
        {
            if(source.Id != id)
            {
                throw new InvalidOperationException("Invalid Id used in request");
            }
            var entity = Get(id);
            if (entity == null)
            {
                throw new Exception(typeof(T).Name);
            }

            _mapper.Map(source, entity);
            _context.Update(entity);
            await _context.SaveChangesAsync();


        }


        public async Task<List<TResult>> GetAllAsync2<TResult>()
        {
            return await _context.Set<T>()
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }


    }
}
