using _90Zone.App.Dto;
using _90Zone.BusinessObjects.Models;
using AutoMapper;

namespace _90Zone.App.Helper {
    public class MappingProfiles : Profile {

        public MappingProfiles() {
            CreateMap<Country, CountryDto>();
            CreateMap<League, LeagueDto>();
            CreateMap<LeagueDto, League>();
            CreateMap<Club, ClubDto>();
            CreateMap<Player, PlayerDto>();
        }
    }
}
