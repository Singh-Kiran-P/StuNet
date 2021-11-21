using System;
using Server.Api.Models;
using System.Collections.Generic;

namespace Server.Api.Controllers // controllers?
{
    public record CourseDto
    {
        public string Name { get; set; }
        public string Number { get; set; }
    }

    public record createCourseDto
    {
        public string Name { get; set; }
        public string Number { get; set; }

        //public ICollection<string> channels {get; set;}

        public ICollection<string> topicNames {get; set;}

        //ublic ICollection<string> assitents {get; set;} 
    }
}