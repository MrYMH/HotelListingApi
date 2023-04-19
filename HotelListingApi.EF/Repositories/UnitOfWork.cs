using AutoMapper;
using HotelHostingApi.EF.Data;
using HotelLisstingApi.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListingApi.EF.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        //inject dbset and props
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ICountryRepository Country { get; private set; }
        public IHotelRepository Hotel { get; private set; }
        //public IAuthManager User { get; private set; }


        public UnitOfWork(ApplicationDbContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            Country = new CountryRepository(_context , _mapper);
            Hotel = new HotelRepository(_context , _mapper);
            //User = new AuthManager(_context , );
        }


        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
