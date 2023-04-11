using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLisstingApi.Core.Dtos.Hotel
{
    public abstract class BaseHotelDto
    {
        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        [Required]
        public double Rating { get; set; }
    }
}
