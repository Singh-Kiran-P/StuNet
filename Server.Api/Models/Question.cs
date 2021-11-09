using System;

namespace Server.Api.Models
{
    public class Question
    {
        public string title { get; set; }
        public string body { get; set; }
        public DateTime dateTime { get; set; }
    }
}