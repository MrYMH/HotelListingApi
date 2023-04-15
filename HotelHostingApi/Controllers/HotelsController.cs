using AutoMapper;
using HotelLisstingApi.Core.Dtos.Country;
using HotelLisstingApi.Core.Dtos.Hotel;
using HotelLisstingApi.Core.IRepositories;
using HotelLisstingApi.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace HotelHostingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiController]
    [EnableQuery]
    public class HotelsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;

        public HotelsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDetailsDto>>> GetHotels()
        {
            var hotels = await _unitOfWork.Hotel.GetAllAsync(null);
            var records = mapper.Map<List<HotelDetailsDto>>(hotels);
            return records;
        }

        // GET: api/Hotels/1
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDetailsDto>> GetHotel(int id)
        {
            var hotel = await _unitOfWork.Hotel.GetFirstAsync(c => c.Id == id);

            if (hotel == null)
            {
                return NotFound();
            }
            var hotelDto = mapper.Map<HotelDetailsDto>(hotel);

            return hotelDto;
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        
        public async Task<IActionResult> PutHotel(int id, HotelDetailsDto hotelDto)
        {

            if (id != hotelDto.Id)
            {
                return BadRequest();
            }
            var hotel = await _unitOfWork.Hotel.GetFirstAsync(c => c.Id == id);
            if (hotel == null)
            {
                return BadRequest();
            }

            mapper.Map(hotelDto, hotel);

            try
            {
                _unitOfWork.Hotel.Update(hotel);
                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        
        public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto hotelDto)
        {
            var hotel = mapper.Map<Hotel>(hotelDto);
            // _context.Countries.Add(country);

            _unitOfWork.Hotel.AddAsync(hotel);

            await _unitOfWork.SaveAsync();

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _unitOfWork.Hotel.GetFirstAsync(c => c.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            _unitOfWork.Hotel.Delete(hotel);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }


        private bool CountryExists(int id)
        {
            //return _context.Countries.Any(e => e.Id == id);
            return _unitOfWork.Hotel.Exists(id);
        }
    }
}
