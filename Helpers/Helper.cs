using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace RentalFinal.Helpers
{
    public static class Helper
    {
        #region SendMail
        public static async Task SendMail(string resetEmailLink, string mailTo)
        {

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.yandex.com";
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("mahammad.b@itbrains.edu.az", "xzzvmvsxjfluyiax");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage message = new MailMessage("mahammad.b@itbrains.edu.az", mailTo);
            message.Subject = "sifre sifirlama";
            message.Body = @$"<h4>Zəhmət olmasa, aşağıdakı linkə klikləməklə parolunuzu sıfırlayın:</h4> 
                                <p><a href='{resetEmailLink}'>Şifrənizi yeniləyin</a></p>";
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            await client.SendMailAsync(message);
        }
        #endregion

        #region Rollar
        public const string Admin = "Admin";
        public const string ContentManager = "ContentManager";
        #endregion

        #region Cinsiyyət
        public enum Gender
        {
            Kişi,
            Qadın,
        }
        #endregion
    }
}
