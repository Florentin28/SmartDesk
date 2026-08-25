using System.Net;
using System.Net.Mail;

namespace SmartDesk.Services;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        // 1. Récupération dynamique depuis appsettings.json
        string host = _configuration["SmtpSettings:Host"] ?? "smtp.gmail.com";
        int port = int.Parse(_configuration["SmtpSettings:Port"] ?? "587");
        string senderEmail = _configuration["SmtpSettings:SenderEmail"]!;
        string password = _configuration["SmtpSettings:Password"]!;

        // 2. Configuration du client SMTP avec les variables récupérées
        using var smtpClient = new SmtpClient(host)
        {
            Port = port,
            Credentials = new NetworkCredential(senderEmail, password),
            EnableSsl = true
        };

        // 3. Construction de l'e-mail
        var mailMessage = new MailMessage
        {
            From = new MailAddress(senderEmail, "SmartDesk Support"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true // pour que l'utilisateur puisse cliquer sur le lien dans le mail
        };
        mailMessage.To.Add(toEmail);

        // 4. Envoi asynchrone
        await smtpClient.SendMailAsync(mailMessage);
    }
}