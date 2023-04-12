using System.ComponentModel.DataAnnotations.Schema;

namespace HotelLisstingApi.Core.Dtos.Hotel
{
    public class HotelDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        
    }
}
