// @kiran

using System.Threading.Tasks;

public interface IEmailSender
{
    Task<bool> SendEmail(string to, string subject, EmailTemplate template, object model);
}
