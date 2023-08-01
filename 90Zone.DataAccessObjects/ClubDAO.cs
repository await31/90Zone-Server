using _90Zone.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _90Zone.DataAccessObjects {
    public class ClubDAO {

        private readonly _90ZoneDbContext _context;
        public ClubDAO(_90ZoneDbContext context) {
            _context = context;
        }

        public List<Club> GetClubs() {
            return _context.Clubs.ToList();
        }
    }
}
