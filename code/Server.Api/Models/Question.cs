using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
namespace Server.Api.Models
{
	public class Question
	{
		public int id { get; set; }
		public string userId { get; set; }
		public Course course { get; set; }
	    public string title { get; set; }
        public string body { get; set; }

        //public IFormFile[] files { get; set; }

		public List<String> filepaths {get;set;}
        public ICollection<Topic> topics { get; set; }
		public DateTime dateTime { get; set; }
    }
}