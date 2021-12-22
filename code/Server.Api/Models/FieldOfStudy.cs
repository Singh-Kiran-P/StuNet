using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Api.Models
{
    public class FieldOfStudy
    {
        [Key]
        public int id { get; set; }
        [Required()]
        public string fullName { get; set; }
        [Required()]
        public string name { get; set; }
        [Required()]
        public bool isBachelor { get; set; }
        [Required()]
        public int year { get; set; }
    }
}
