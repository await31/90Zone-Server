using _90Zone.BusinessObjects.Models;

namespace _90Zone.Repositories {
    public class ClubRepository : IClubRepository {

        private readonly _90ZoneDbContext _context;
        public ClubRepository(_90ZoneDbContext context) {
            _context = context;
        }

        public ICollection<Club> GetClubs() {
            return _context.Clubs.ToList();
        }
    }
}

