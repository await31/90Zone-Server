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
        private readonly IMapper _mapper;

        public LeagueController(ILeagueRepository leagueRepository, IMapper mapper)
        {
            _leagueRepository = leagueRepository;
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
        public IActionResult CreateLeague([FromQuery] int countryId, [FromBody] LeagueDto leagueCreate) {
            if (leagueCreate == null)
                return BadRequest(ModelState);

            var league = _leagueRepository.GetLeague(leagueCreate.Id);

            if (league != null) {
                ModelState.AddModelError("", "League already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var leagueMap = _mapper.Map<League>(leagueCreate);

            if (!_leagueRepository.CreateLeague(leagueMap, countryId)) {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
    }
}
