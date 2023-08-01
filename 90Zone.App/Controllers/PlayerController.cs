using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using _90Zone.Repositories;
using _90Zone.BusinessObjects.Models;
using AutoMapper;
using _90Zone.App.Dto;

namespace _90Zone.App.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase {

        private readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;
        public PlayerController(IPlayerRepository playerRepository, IMapper mapper) {
            _playerRepository = playerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Player>))]
        public IActionResult GetPlayers() {
            var players = _mapper.Map<List<PlayerDto>>(_playerRepository.GetPlayers());
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(players);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Player))]
        [ProducesResponseType(400)]
        public IActionResult GetPlayer(int id) {
            if (!_playerRepository.PlayerExist(id))
                return NotFound();
            var player = _mapper.Map<PlayerDto>(_playerRepository.GetPlayer(id));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(player);
        }
    }
}
