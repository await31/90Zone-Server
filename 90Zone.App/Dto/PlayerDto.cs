namespace _90Zone.App.Dto {
    public class PlayerDto {
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
    }
}
