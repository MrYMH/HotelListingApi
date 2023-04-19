using AutoMapper;
using HotelHostingApi.EF.Data;
using HotelLisstingApi.Core.IRepositories;
using HotelLisstingApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListingApi.EF.Repositories
{
    public class CountryRepository : BaseRepository<Country> , ICountryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CountryRepository(ApplicationDbContext context , IMapper mapper ) : base( context  , mapper)
        {
            _context= context;
            _mapper= mapper;
        }
    }
}
