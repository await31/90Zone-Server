using _90Zone.BusinessObjects.Models;


namespace _90Zone.Repositories {
    public interface IClubRepository {
        ICollection<Club> GetClubs();
    }
}
