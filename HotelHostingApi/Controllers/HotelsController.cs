using AutoMapper;
using HotelLisstingApi.Core.Dtos.Country;
using HotelLisstingApi.Core.Dtos.Hotel;
using HotelLisstingApi.Core.IRepositories;
using HotelLisstingApi.Core.Models;
using HotelListingApi.EF.Repositories;
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
        
        private readonly IUnitOfWork _unitOfWork;

        public HotelsController(IUnitOfWork unitOfWork)
        {
            
            _unitOfWork = unitOfWork;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDetailsDto>>> GetHotels()
        {
            var records = await _unitOfWork.Hotel.GetAllAsync<HotelDetailsDto>();
            return records;
        }

        // GET: api/Hotels/1
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<HotelDetailsDto>> GetHotel(int id)
        {
            var result = await _unitOfWork.Hotel.GetFirstAsync<HotelDetailsDto>(c => c.Id == id);
            return result;
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutHotel(int id, HotelDetailsDto hotelDto)
        {
            try
            {
                await _unitOfWork.Hotel.UpdateAsync(id, hotelDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
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
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<HotelDetailsDto>> PostHotel(CreateHotelDto hotelDto)
        {
            
            var hotel = await _unitOfWork.Hotel.AddAsync<CreateHotelDto, HotelDetailsDto>(hotelDto);
            return CreatedAtAction(nameof(HotelDetailsDto), new { id = hotel.Id }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _unitOfWork.Hotel.GetFirstAsync<Hotel>(c => c.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            _unitOfWork.Hotel.Delete(hotel);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }


        private bool HotelExists(int id)
        {
            return _unitOfWork.Hotel.Exists(id);
        }
    }
}
