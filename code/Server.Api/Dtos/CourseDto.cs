using Server.Api.Models;
using System.Collections.Generic;

namespace Server.Api.Dtos
{
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
        public ICollection<GetPartialChannelDto> channels;

        public static GetCourseDto Convert(Course course)
        {
            throw new System.Exception("method not implement");
        }
    }

    public record GetPartialCourseDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string description { get; set; }

        public static GetPartialCourseDto Convert(Course course)
        {
            return new GetPartialCourseDto
            {
                id = course.id,
                name = course.name,
                number = course.number,
                description = course.description
            };
        }
    }

    public record CreateCourseDto
    {
        public string name { get; set; }
        public string number { get; set; }
        public string description { get; set; }

        public static CreateCourseDto Convert(Course course)
        {
            throw new System.Exception("method not implement");
        }
    }
}
