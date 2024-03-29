using Microsoft.AspNet.Identity;
using System.Net.Mail;


public class EmailService
{

    public Task SendAsync(IdentityMessage message)
    {
        var from = "spacyworktesting@yandex.ru";
        var pass = "zschomohgecqcucc";

        SmtpClient client = new SmtpClient("smtp.yandex.ru", 587);

        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        client.Credentials = new System.Net.NetworkCredential(from, pass);
        client.EnableSsl = true;

        var mail = new MailMessage(from, message.Destination);
        mail.Subject = message.Subject;
        mail.Body = message.Body;
        mail.IsBodyHtml = true;

        return client.SendMailAsync(mail);

    }
}
