using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLisstingApi.Core.IRepositories
{
    public interface IUnitOfWork
    {
        //props = tables
        ICountryRepository Country { get; }
        IHotelRepository Hotel { get; }
        //IAuthManager User { get; }


        //methods
        void Save();
        Task SaveAsync();
    }
}
