using System.Net;
using System.Net.Mail;

namespace SmartDesk.Services;

public class EmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        // 1. Configuration des identifiants d'envoi
        var fromEmail = "muraillef@gmail.com"; 
        var appPassword = "cptz oaea wpvh iazj"; // Le mot de passe d'application généré

        // 2. Configuration du client SMTP de Google
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,                                 // Port standard pour l'envoi sécurisé TLS
            Credentials = new NetworkCredential(fromEmail, appPassword),
            EnableSsl = true                            // Chiffrement obligatoire par Google
        };

        // 3. Construction de l'e-mail
        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail, "SmartDesk Support"),
            Subject = subject,
            Body = body,
            IsBodyHtml = false                          // Texte brut (mettre true si on utilise des balises HTML)
        };
        mailMessage.To.Add(toEmail);

        // 4. Envoi asynchrone
        await smtpClient.SendMailAsync(mailMessage);
    }
}