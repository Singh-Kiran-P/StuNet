using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Api.Dtos
{
    public record FieldOfStudyDto
    {
        public string fullName { get; set; }
        [Required()]
        public string name { get; set; }
        [Required()]
        public bool isBachelor { get; set; }
    }
}
