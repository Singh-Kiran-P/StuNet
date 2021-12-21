using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Api.Models
{
    public class Answer
    {
        public int id { get; set; }
        public string userId { get; set; }
        public int questionId { get; set; }
		public virtual Question question { get; set; }
	    public string title { get; set; }
        public string body { get; set; }
        // public File[] files { get; set; }
		public DateTime time { get; set; }
    }
}