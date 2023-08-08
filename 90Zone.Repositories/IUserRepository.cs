using _90Zone.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _90Zone.Repositories {
    public interface IUserRepository {
        ICollection<User> GetUsers();
        bool Save();

    }
}
