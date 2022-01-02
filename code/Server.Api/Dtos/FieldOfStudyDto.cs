namespace Server.Api.Dtos
{
    public record FieldOfStudyDto
    {
        public string name { get; set; }
        public bool isBachelor { get; set; }
        public string fullName { get; set; }
    }
    
    public record CreateFieldOfStudyDto
    {
        public string name { get; set; }
        public bool isBachelor { get; set; }
        public string fullName { get; set; }
    }
}
