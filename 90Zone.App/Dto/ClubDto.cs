using _90Zone.BusinessObjects.Models;

namespace _90Zone.App.Dto {
    public class ClubDto {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImgPath { get; set; }
        public string? Stadium { get; set; }
        public string? Chairmen { get; set; }
        public string? Coach { get; set; }
    }
}
