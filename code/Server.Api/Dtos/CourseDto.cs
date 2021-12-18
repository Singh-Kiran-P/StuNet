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

    public record GetAllCourseDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public ICollection<getOnlyTopicDto> topics;
        
    }

    public record GetCourseDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public ICollection<getOnlyTopicDto> topics;
		public ICollection<getOnlyChannelDto> channels;
		public ICollection<onlyQuestionDto> questions;
    }

    public record getOnlyCourseDto {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }

        public static getOnlyCourseDto convert(Course course){
            return new getOnlyCourseDto{
                    id = course.id,
                    name = course.name,
                    number = course.number
            };
        }
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