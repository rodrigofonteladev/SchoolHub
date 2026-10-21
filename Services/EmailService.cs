using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SchoolHub.Common;
using SchoolHub.Common.Settings;
using SchoolHub.Interfaces;

namespace SchoolHub.Services
{
    public class EmailService(
        ILogger<EmailService> logger,
        IOptions<SmtpSettings> smtpOptions) : IEmailService
    {
        private readonly SmtpSettings _settings = smtpOptions.Value;

        public async Task<Result> SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
                message.To.Add(MailboxAddress.Parse(toEmail));
                message.Subject = subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = htmlBody
                };
                message.Body = builder.ToMessageBody();

                using var client = new SmtpClient();

                await client.ConnectAsync(_settings.Server, _settings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_settings.UserName, _settings.Password);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                logger.LogInformation($"Email sent to: {toEmail}");

                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error sending email to: {toEmail}");
                return Result.Failure($"The email could not be sent: {ex.Message}");
            }
        }

        public async Task<Result> SendStudentPasswordResetLinkAsync(
            string toEmail,
            string personName,
            string userName,
            string userEmail,
            string resetLink)
        {
            string subject = "Bienvenido a SchoolHub - Activa tu cuenta";

            string htmlBody = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                    <h2>¡Bienvenido(a), {personName}!</h2>
                    <p>Tu cuenta en <strong>SchoolHub</strong> ha sido creada satisfactoriamente.</p>
                    <div style='background-color: #f4f6f9; border-left: 4px solid #0056b3; padding: 15px; margin: 20px 0;'>
                        <p style='margin: 0 0 8px;'><strong>Usuario institucional:</strong> {userName}</p>
                        <p style='margin: 0 0 8px;'><strong>Correo electrónico institucional:</strong> {userEmail}</p>
                    </div>
                    <p>Utiliza el siguiente botón para establecer tu contraseña y activar tu acceso:</p>
                    <p><a href='{resetLink}' style='display: inline-block; background-color: #0056b3; color: #fff; padding: 10px 18px; text-decoration: none; border-radius: 4px;'>Establecer contraseña</a></p>
                    <p><em>Por seguridad, este enlace es personal y debe utilizarse únicamente para configurar tu contraseña.</em></p>
                    <p>Atentamente,<br/>Administración SchoolHub</p>
                </div>
            ";

            return await SendEmailAsync(toEmail, subject, htmlBody);
        }
    }
}
