using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using _90Zone.Repositories;
using _90Zone.BusinessObjects.Models;
using AutoMapper;
using _90Zone.App.Dto;

namespace _90Zone.App.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ClubController : ControllerBase {

        private readonly IClubRepository _clubRepository;
        private readonly IMapper _mapper;

        public ClubController(IClubRepository clubRepository, IMapper mapper) {
             _clubRepository = clubRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type= typeof(ICollection<Club>))]
        public IActionResult GetClubs() {
            var clubs = _mapper.Map<List<ClubDto>>(_clubRepository.GetClubs());
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(clubs);
        }
    }
}
