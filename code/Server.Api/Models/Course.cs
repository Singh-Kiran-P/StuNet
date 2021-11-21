using System;
using Server.Api.Models;
using System.Collections.Generic;
namespace Server.Api.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }

        public ICollection<Topic> topics { get; set; }
    }
}