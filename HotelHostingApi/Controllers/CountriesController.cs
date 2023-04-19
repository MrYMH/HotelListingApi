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
        //Administrator
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            var records = await _unitOfWork.Country.GetAllAsync<GetCountryDto>();
            return records ;
        }

        // GET: api/Countries/1
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDetailsDto>> GetCountry(int? id)
        {
            var records = await _unitOfWork.Country.GetFirstAsync<CountryDetailsDto>(c => c.Id == id, "Hotels");
            return records;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        
        public async Task<IActionResult> PutCountry(int id, GetCountryDto countryDto)
        {
            try
            {
                await _unitOfWork.Country.UpdateAsync<GetCountryDto>(id , countryDto);
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
        public async Task<ActionResult<GetCountryDto>> PostCountry(CreateCountryDto createCountryDto)
        {
            var record = await _unitOfWork.Country.AddAsync<CreateCountryDto , GetCountryDto>(createCountryDto);
            return CreatedAtAction(nameof(CreateCountryDto), new { id = record.Id }, record);
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
            return _unitOfWork.Country.Exists(id);
        }
    }
}
