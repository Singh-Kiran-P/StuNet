using Server.Api.Models;
using System.Collections.Generic;

namespace Server.Api.Dtos
{
    public record GetTopicDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public GetPartialCourseDto course { get; set; }
        public ICollection<GetPartialQuestionDto> questions { get; set; }
    }

    public record GetPartialTopicDto
    {
        public int id { get; set; }
        public string name { get; set; }

        public static GetPartialTopicDto Convert(Topic topic)
        {
            return new GetPartialTopicDto {
                id = topic.id,
                name = topic.name
            };
        }
    }

    public record CreateTopicDto
    {
        public string name { get; set; }
        public int courseId { get; set; }
    }
}
