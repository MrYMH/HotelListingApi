using System.ComponentModel.DataAnnotations.Schema;

namespace HotelLisstingApi.Core.Dtos.Hotel
{
    public class HotelDetailsDto : BaseHotelDto, IBaseDto
    {
        public int Id { get; set; }
        
    }
}
