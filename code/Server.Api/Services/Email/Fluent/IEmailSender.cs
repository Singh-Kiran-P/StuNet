// @kiran

using System.Threading.Tasks;

public interface IEmailSender //TOASK: Move to namespace?
{
    Task<bool> SendEmail(string to, string subject, EmailTemplate template, object model);
}
