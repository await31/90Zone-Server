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

            var country = _context.Countries.FirstOrDefault(a=>a.Id==countryId);

            if (country == null) {
                return false;
            }

            league.Country = country;
            _context.Add(league);
            return Save();
        }

        public bool UpdateLeague(League updatedLeague) {
            var existingLeague = _context.Leagues
                .Include(a => a.Country)
                .FirstOrDefault(a => a.Id == updatedLeague.Id);
            if (existingLeague == null) {
                return false;
            }
            existingLeague.Name = updatedLeague.Name;
            existingLeague.Country = updatedLeague.Country;
            _context.Update(existingLeague);
            return Save();
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
