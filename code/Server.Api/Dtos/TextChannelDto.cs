using System;
using Server.Api.Dtos;
using System.Collections.Generic;
namespace Server.Api.Models
{
    public class getOnlyChannelDto
    {
        public int id { get; set; }
        public string name { get; set; }
		// public ICollection<getOnlyMessagesDto> messages { get; set; }
    }

	public class createChannelDto
	{
		public string name { get; set; }
		public int courseId { get; set; }
	}
}
