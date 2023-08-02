using _90Zone.App.Dto;
using _90Zone.BusinessObjects.Models;
using _90Zone.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _90Zone.App.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : ControllerBase {

        private readonly ILeagueRepository _leagueRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public LeagueController(ILeagueRepository leagueRepository, ICountryRepository countryRepository, IMapper mapper) {
            _leagueRepository = leagueRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<League>))]
        public IActionResult GetLeagues() {
            var leagues = _mapper.Map<List<LeagueDto>>(_leagueRepository.GetLeagues());
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(leagues);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(League))]
        [ProducesResponseType(400)]
        public IActionResult GetLeague(int id) {
            if (!_leagueRepository.LeagueExist(id))
                return NotFound();
            var league = _mapper.Map<LeagueDto>(_leagueRepository.GetLeague(id));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(league);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public JsonResult CreateLeague(League league, int countryId) {
            _leagueRepository.CreateLeague(league, countryId);
            return new JsonResult("League created successfully");
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public JsonResult UpdateLeague(League league, int countryId) {
            _leagueRepository.UpdateLeague(league, countryId);
            return new JsonResult("League created successfully");
        }
    }
}
