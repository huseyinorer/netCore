using MainProject.Models;
using System.Linq;
using System.Net.Mail;

namespace MainProject.Helper
{
    public static class SendEmail
    {
        public static void SendCallBackURL(string Email, string URL,ProjectDbContext db)
        {
            var settings = db.Settings.SingleOrDefault();
            var webSiteName = settings.WebSiteName;
            var networkCredentinal_Username = settings.Credentials_Email;
            var networkCredentinal_Password = settings.Credentials_Password;

            MailMessage mail = new MailMessage();
            
            mail.From = new MailAddress("unknownartistim@gmail.com");

            mail.To.Add(Email);
            mail.Subject = webSiteName;
            mail.Body = "<h2>Şifre yenileme linki</h2><hr/>";
            mail.Body += $"<a href='{URL}'> Şifre yenileme linki</a>";
            mail.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(networkCredentinal_Username, networkCredentinal_Password);
            smtpClient.EnableSsl = true;

            smtpClient.Send(mail);
        }
    }
}