using System;

namespace Server.Api.Models
{
    public class FieldOfStudy
    {
        public int id { get; set; }
        public string fullName { get; set; }
        public string name { get; set; }
        public bool isBachelor { get; set; }
        public int year { get; set; }
    }
}