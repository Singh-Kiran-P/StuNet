using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Api.Dtos
{
    public record createFieldOfStudyDto
    {
        [Required()]
        public string name { get; set; }
        [Required()]
        public bool isBachelor { get; set; }
        [Required()]
        public int year { get; set; }
    }
}
