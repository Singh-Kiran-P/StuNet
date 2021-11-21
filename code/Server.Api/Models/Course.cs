using System;
using Server.Api.Models;
using System.Collections.Generic;
namespace Server.Api.Models
{
    public class Course
    {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }

        public ICollection<Topic> topics { get; set; }
    }
}