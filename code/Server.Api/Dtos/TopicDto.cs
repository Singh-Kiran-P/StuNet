using System;
using Server.Api.Models;

namespace Server.Api.Controllers
{
    public record TopicDto
    {
		public string name { get; set; }
		// public int courseID { get; set; }
	}
}