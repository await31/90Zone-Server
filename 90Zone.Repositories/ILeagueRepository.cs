using _90Zone.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _90Zone.Repositories {
    public interface ILeagueRepository {
        ICollection<League> GetLeagues();
        League GetLeague(int id);
        bool LeagueExist(int id);
        bool CreateLeague(League league, int countryId);
        bool UpdateLeague(League existingLeague);
        bool Save();
    }
}
