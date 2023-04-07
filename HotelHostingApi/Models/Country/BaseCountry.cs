using System.ComponentModel.DataAnnotations;

namespace HotelHostingApi.Models.Country
{
    public abstract class BaseCountry
    {
        //3-4-16-17-
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
