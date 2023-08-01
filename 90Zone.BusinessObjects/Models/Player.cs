using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _90Zone.BusinessObjects.Models {
    public class Player {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime Birthday { get; set; }
        public string? Position { get; set; }
        public int? Age { get; set; }
        public double? Value { get; set; }
        public int? JerseyNumber { get; set; }
        public int? Height { get; set; } //cm
        public int? Weight { get; set; } //kg
        public bool? IsInjured { get; set; }
        public string? ImgPath { get; set; }
        public virtual Country? Nationality { get; set; }
        public virtual Club? CurrentClub { get; set; }
    }
}
