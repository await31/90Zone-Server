
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _90Zone.BusinessObjects.Models {
    public class Country {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Continent { get; set; }
        public virtual ICollection<Player> Players { get; set; } = new List<Player>();
        public virtual ICollection<League> Leagues { get; set; } = new List<League>();
    }
}
