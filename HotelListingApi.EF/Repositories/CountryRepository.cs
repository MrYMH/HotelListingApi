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
        public CountryRepository(ApplicationDbContext context ) : base( context )
        {
            _context= context;
        }
    }
}
