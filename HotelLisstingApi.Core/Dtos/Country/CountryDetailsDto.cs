﻿using HotelLisstingApi.Core.Dtos.Hotel;

namespace HotelLisstingApi.Core.Dtos.Country
{
    public class CountryDetailsDto : BaseCountry, IBaseDto
    {
        public int Id { get; set; }
       
        public virtual IList<HotelDetailsDto> Hotels { get; set; }
    }
}
