using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using _90Zone.Repositories;
using _90Zone.BusinessObjects.Models;
using AutoMapper;
using _90Zone.App.Dto;

namespace _90Zone.App.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase {

        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepository countryRepository, IMapper mapper) {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Country>))]
        public IActionResult GetCountries() {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(countries);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry([FromRoute] int id) {
            if (!_countryRepository.CountryExist(id))
                return NotFound();
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(id));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] Country country) {
            _countryRepository.CreateCountry(country);
            return Ok(country);
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult EditCountry([FromRoute] int id, Country updateCountryRequest) {
            if (!_countryRepository.CountryExist(id))
                return NotFound();

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var country = _countryRepository.GetCountry(id);

            _countryRepository.EditCountry(id, updateCountryRequest);
            return Ok(country);
        }

        [HttpGet("league/{countryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<League>))]
        [ProducesResponseType(400)]
        public IActionResult GetLeagueByCountry(int countryId) {
            if (!_countryRepository.CountryExist(countryId))
                return NotFound();

            var leagues = _mapper.Map<List<LeagueDto>>(
                _countryRepository.GetLeagueByCountry(countryId));

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(leagues);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCountry([FromRoute] int id) {
            if (!_countryRepository.CountryExist(id))
                return NotFound();

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var country = _countryRepository.GetCountry(id);

            _countryRepository.DeleteCountry(id);

            return Ok(country);
        }
    }
}
