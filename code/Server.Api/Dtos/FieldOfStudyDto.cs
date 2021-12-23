using System.ComponentModel.DataAnnotations;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public record FieldOfStudyDto
    {
        public string fullName { get; set; }
        [Required()]
        public string name { get; set; }
        [Required()]
        public bool isBachelor { get; set; }

        public static FieldOfStudyDto Convert(FieldOfStudy fieldOfStudy)
        {
            throw new System.Exception("method not implement");
        }
    }
    
    public record CreateFieldOfStudyDto
    {
        [Required()]
        public string name { get; set; }
        [Required()]
        public bool isBachelor { get; set; }

        public static CreateFieldOfStudyDto Convert(FieldOfStudy fieldOfStudy)
        {
            throw new System.Exception("method not implement");
        }
    }
}
