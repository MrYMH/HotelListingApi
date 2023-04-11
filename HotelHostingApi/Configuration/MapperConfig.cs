using AutoMapper;
using HotelLisstingApi.Core.Dtos.Country;
using HotelLisstingApi.Core.Dtos.Hotel;
using HotelLisstingApi.Core.Models;

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
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();

        }
    }
}
