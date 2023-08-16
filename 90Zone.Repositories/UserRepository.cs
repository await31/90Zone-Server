using _90Zone.BusinessObjects.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _90Zone.Repositories {
    public class UserRepository : IUserRepository {

        private readonly _90ZoneDbContext _context;

        public UserRepository(_90ZoneDbContext context) {
            _context = context;
        }

        public ICollection<User> GetUsers() {
            return _context.Users
                .ToList();
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
