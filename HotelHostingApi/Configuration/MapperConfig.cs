using AutoMapper;
using HotelHostingApi.Data;
using HotelHostingApi.Models.Country;
using HotelHostingApi.Models.Hotel;

namespace HotelHostingApi.Configuration
{
    public class MapperConfig :Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country, CountryDetailsDto>().ReverseMap();

            CreateMap<Hotel, HotelDetailsDto>().ReverseMap();

        }
    }
}
