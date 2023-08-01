using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _90Zone.BusinessObjects.Models {
    public class Club {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImgPath { get; set; }
        public string? Stadium { get; set; }
        public string? Chairmen { get; set; }
        public string? Coach { get; set; }
        public virtual Manager? Manager { get; set; }
        public virtual League? League { get; set; }
        public virtual ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
