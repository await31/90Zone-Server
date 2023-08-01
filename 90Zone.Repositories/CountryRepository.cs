using _90Zone.BusinessObjects.Models;

namespace _90Zone.Repositories {
    public class CountryRepository : ICountryRepository {

        private readonly _90ZoneDbContext _context;
        public CountryRepository(_90ZoneDbContext context) {
            _context = context;
        }

        public ICollection<Country> GetCountries() {
            return _context.Countries.ToList();
        }

        public Country GetCountry(int id) {
            var country = _context.Countries.FirstOrDefault(a => a.Id == id);
            return country;
        }
        public ICollection<League> GetLeagueByCountry(int countryId) {
            return _context.Countries.Where(a=>a.Id == countryId).SelectMany(a=>a.Leagues).ToList();
        }

        public bool CountryExist(int id) {
            return _context.Countries.Any(a => a.Id == id);
        }
    }
}

