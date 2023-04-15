using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using HotelLisstingApi.Core.Dtos.Country;
using HotelLisstingApi.Core.Models;
using HotelHostingApi.EF.Data;
using HotelLisstingApi.Core.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Query;

namespace HotelHostingApi.Controllers
{
    [Route("api/v{version:apiVersion}/countries")]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiController]
    [EnableQuery]
    public class CountriesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CountriesController(IMapper mapper , IUnitOfWork unitOfWork )
        {
            this.mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            var countries =  await _unitOfWork.Country.GetAllAsync(null);
            var records = mapper.Map<List<GetCountryDto>>(countries);
            return records ;
        }

        // GET: api/Countries/1
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDetailsDto>> GetCountry(int? id)
        {
            var country = await _unitOfWork.Country.GetFirstAsync(c=>c.Id == id ,"Hotels");

            if (country == null)
            {
                return NotFound();
            }
            var countryDto = mapper.Map<CountryDetailsDto>(country);

            return countryDto;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        
        public async Task<IActionResult> PutCountry(int id, GetCountryDto countryDto)
        {

            if (id != countryDto.Id)
            {
                return BadRequest();
            }
            var country = await _unitOfWork.Country.GetFirstAsync(c => c.Id == id);
            if (country == null)
            {
                return BadRequest();
            }

            mapper.Map(countryDto, country);

            try
            {
                //await _countriesRepository.UpdateAsync(country);
                _unitOfWork.Country.Update(country);
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



        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
        {
            var country = mapper.Map<Country>(createCountryDto);
            // _context.Countries.Add(country);

            _unitOfWork.Country.AddAsync(country);

            await _unitOfWork.SaveAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }




        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _unitOfWork.Country.GetFirstAsync(c => c.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            _unitOfWork.Country.Delete(country);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            //return _context.Countries.Any(e => e.Id == id);
            return _unitOfWork.Country.Exists(id);
        }
    }
}
