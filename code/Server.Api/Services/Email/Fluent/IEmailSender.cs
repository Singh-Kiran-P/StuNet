using System.Threading.Tasks;

public interface IEmailSender //TOASK: Move to namespace?
{
    Task<bool> SendUsingTemplate(string to, string subject, EmailTemplate template, object model);
    void SendEmail(string email, string subject, string message);
}
