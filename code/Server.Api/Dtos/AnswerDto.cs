using System;
using Server.Api.Models;
using System.Collections.Generic;

namespace Server.Api.Dtos
{
    public record ResponseAnswerDto {
        public int id { get; set; }
        public ResponseUserDto user { get; set; }
        public onlyQuestionUserDto question { get; set; }
        public getOnlyCourseDto course { get; set; }
		public string title { get; set; }
        public string body { get; set; }
        public DateTime dateTime { get; set; }
        public static ResponseAnswerDto convert(Answer answer, User user) {
			return new ResponseAnswerDto
			{
				id = answer.id,
				user = ResponseUserDto.convert(user),
				question = onlyQuestionUserDto.convert(answer.question),
                course = getOnlyCourseDto.convert(answer.question.course),
				title = answer.title,
				body = answer.body,
				dateTime = answer.dateTime
			};
		}
	}

    public record PostAnswerDto {
        public string userId {get;set;}
        public int questionId {get;set;}
        public string title { get; set; }
        public string body { get; set; }    
        
    }
}