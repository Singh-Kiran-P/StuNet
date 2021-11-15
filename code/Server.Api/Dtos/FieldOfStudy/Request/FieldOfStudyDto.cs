using System;

namespace Server.Api.Dtos
{
    public record FieldOfStudyDto
    {
        public string fullName { get; set; } = null;
        public string name { get; set; }
        public bool isBachelor { get; set; }
        public int year { get; set; }
    }
}