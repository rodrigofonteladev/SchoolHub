using SchoolHub.Common;

namespace SchoolHub.Interfaces
{
    public interface IEmailService
    {
        Task<Result> SendEmailAsync(string toEmail, string subject, string htmlBody);
        Task<Result> SendStudentPasswordResetLinkAsync(string toEmail, string personName, string userName, string userEmail, string resetLink);
    }
}
