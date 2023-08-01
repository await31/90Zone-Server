using _90Zone.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace _90Zone.Repositories {
    public class PlayerRepository : IPlayerRepository {

        private readonly _90ZoneDbContext _context;
        public PlayerRepository(_90ZoneDbContext context) {
            _context = context;
        }


        public Player GetPlayer(int id) => _context.Players.FirstOrDefault(a => a.Id == id);

        public ICollection<Player> GetPlayers() => _context.Players.ToList();

        public bool PlayerExist(int id) => _context.Players.Any(a=>a.Id == id);
    }
}
