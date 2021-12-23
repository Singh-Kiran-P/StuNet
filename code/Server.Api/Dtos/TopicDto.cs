using System.Collections.Generic;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public record createTopicDto
    {
        public string name { get; set; }
        public int courseId { get; set; }

        public static createTopicDto Convert(Topic topic)
        {
            throw new System.Exception("method not implement");
        }
    }

    public record getOnlyTopicDto
    {
        public int id { get; set; }
        public string name { get; set; }

        public static getOnlyTopicDto Convert(Topic topic)
        {
            return new getOnlyTopicDto
                {
                    id = topic.id,
                    name = topic.name
                };
        }
    }

    public record getTopicDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public GetPartialCourseDto course { get; set; }
        public ICollection<onlyQuestionDto> questions { get; set; }

        public static getTopicDto Convert(Topic topic)
        {
            throw new System.Exception("method not implement");
        }
    }
}
