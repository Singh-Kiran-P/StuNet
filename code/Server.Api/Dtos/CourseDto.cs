using System.Linq;
using Server.Api.Models;
using System.Collections.Generic;

namespace Server.Api.Dtos
{
    public record GetAllCourseDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string profEmail { get; set; }
        public string description { get; set; }
        public string courseEmail { get; set; }
        public ICollection<GetPartialTopicDto> topics;

        public static GetAllCourseDto Convert(Course course)
        {
            return new GetAllCourseDto() {
                id = course.id,
                name = course.name,
                number = course.number,
                profEmail = course.profEmail,
                description = course.description,
                courseEmail = course.courseEmail,
                topics = course.topics.Select(topic => new GetPartialTopicDto() { name = topic.name, id = topic.id }).ToList()
            };
        }
    }

    public record GetCourseDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string profEmail { get; set; }
        public string description { get; set; }
        public string courseEmail { get; set; }
        public ICollection<GetPartialTopicDto> topics;
        public ICollection<GetPartialChannelDto> channels;

        public static GetCourseDto Convert(Course course)
        {
            return new() {
                id = course.id,
                name = course.name,
                number = course.number,
                profEmail = course.profEmail,
                description = course.description,
                courseEmail = course.courseEmail,
                topics = course.topics.Select(topic => new GetPartialTopicDto(){ id = topic.id, name = topic.name }).ToList(),
                channels = course.channels.Select(channel => new GetPartialChannelDto(){ id = channel.id, name = channel.name }).ToList()
            };
        }
    }

    public record GetPartialCourseDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string profEmail { get; set; }
        public string description { get; set; }
        public string courseEmail { get; set; }
        public static GetPartialCourseDto Convert(Course course)
        {
            return new GetPartialCourseDto {
                id = course.id,
                name = course.name,
                number = course.number,
                profEmail = course.profEmail,
                courseEmail = course.courseEmail,
                description = course.description
            };
        }
    }

    public record CreateCourseDto
    {
        public string name { get; set; }
        public string number { get; set; }
        public string profEmail { get; set; }
        public string description { get; set; }
        public string courseEmail { get; set; }
    }
}
