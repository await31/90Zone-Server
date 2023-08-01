using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _90Zone.BusinessObjects.Models {
    public class League {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public virtual Country? Country { get; set; }
        public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();
    }
}
