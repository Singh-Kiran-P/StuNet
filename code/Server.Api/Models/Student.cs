
namespace Server.Api.Models
{
    public class Student : User
    {
        //Todo: add field of study, maybe enum?
        public int FieldOfStudyId { get; set; }
    }
}
