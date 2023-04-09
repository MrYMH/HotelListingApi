using HotelHostingApi.Data;
using HotelHostingApi.Models.Hotel;

namespace HotelHostingApi.Models.Country
{
    public class CountryDetailsDto : BaseCountry
    {
        public int Id { get; set; }
       
        public virtual IList<HotelDetailsDto> Hotels { get; set; }
    }
}
