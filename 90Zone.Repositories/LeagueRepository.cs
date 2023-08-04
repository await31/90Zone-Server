using _90Zone.BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _90Zone.Repositories {
    public class LeagueRepository : ILeagueRepository {

        private readonly _90ZoneDbContext _context;

        public LeagueRepository(_90ZoneDbContext context) {
            _context = context;
        }

        public ICollection<League> GetLeagues() => _context.Leagues
            .Include(a=>a.Country)
            .ToList();
        public League GetLeague(int id) {
            return _context.Leagues
                .Include(a=>a.Country)
                .FirstOrDefault(a=>a.Id == id);
        }

        public bool LeagueExist(int id) {
            return _context.Leagues.Any(a => a.Id == id);
        }

        public bool CreateLeague(League league, int countryId) {
            var country = _context.Countries.FirstOrDefault(c => c.Id == countryId);
            league.Country = country;
            _context.Add(league);
            return Save();
        }

        public bool UpdateLeague(int id, League leagueUpdateRequest, int countryId) {

            var country = _context.Countries.FirstOrDefault(c => c.Id == countryId);

            var league = _context.Leagues.FirstOrDefault(c=>c.Id== id);

            if(league!=null && country != null) {
                league.Name = leagueUpdateRequest.Name;
                league.Country = country;
            }

            return Save();
        }

        public bool DeleteLeague(int id) {
            var league = _context.Leagues.FirstOrDefault(a => a.Id == id);
            _context.Remove(league);
            return Save();
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
