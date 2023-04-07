using AutoMapper;
using HotelHostingApi.Data;
using HotelHostingApi.Models.Country;

namespace HotelHostingApi.Configuration
{
    public class MapperConfig :Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap();
        }
    }
}
