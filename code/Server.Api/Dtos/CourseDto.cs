using Server.Api.Models;
using System.Collections.Generic;

namespace Server.Api.Dtos
{
    public record CourseDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string description { get; set; }

        public static CourseDto Convert(Course course)
        {
            return new CourseDto
            {
                id = course.id,
                name = course.name,
                number = course.number,
                description = course.description
            };
        }
    }

    public record GetAllCourseDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string description { get; set; }
        public ICollection<getOnlyTopicDto> topics;

        public static GetAllCourseDto Convert(Course course)
        {
            throw new System.Exception("method not implement");
        }
    }

    public record GetCourseDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string description { get; set; }
        public ICollection<getOnlyTopicDto> topics;
        public ICollection<getOnlyChannelDto> channels;

        public static GetCourseDto Convert(Course course)
        {
            throw new System.Exception("method not implement");
        }
    }

    public record getOnlyCourseDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string description { get; set; }

        public static getOnlyCourseDto Convert(Course course)
        {
            return new getOnlyCourseDto
            {
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
        public string description { get; set; }

        public static createCourseDto Convert(Course course)
        {
            throw new System.Exception("method not implement");
        }
    }
}
