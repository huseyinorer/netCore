using System.Net.Mail;

namespace MainProject.Helper
{
    public static class SendEmail
    {
        public static void SendCallBackURL(string Email, string URL)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("unknownartistim@gmail.com");

            mail.To.Add(Email);
            mail.Subject = "mvc test maili";
            mail.Body = "<h2>Şifre yenileme linki</h2><hr/>";
            mail.Body += $"<a href='{URL}'> Şifre yenileme linki</a>";
            mail.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("unknownartistim@gmail.com", "fadwrgwfcbinfckt");
            smtpClient.EnableSsl = true;

            smtpClient.Send(mail);
        }
    }
}