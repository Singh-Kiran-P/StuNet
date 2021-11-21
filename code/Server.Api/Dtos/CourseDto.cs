using System;
using Server.Api.Models;
using System.Collections.Generic;

namespace Server.Api.Dtos // controllers?
{
    public record CourseDto
    {
        public string name { get; set; }
        public string number { get; set; }
    }

    public record GetCourseDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public ICollection<getOnlyTopicDto> topics;
    }

    public record getOnlyCourseDto {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
    }

    public record createCourseDto
    {
        public string name { get; set; }
        public string number { get; set; }

        //public ICollection<string> channels {get; set;}

        public ICollection<string> topicNames {get; set;}

        //ublic ICollection<string> assitents {get; set;} 
    }
}