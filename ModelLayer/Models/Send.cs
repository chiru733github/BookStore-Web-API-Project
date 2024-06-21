using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Model_Layer.Models
{
    public class Send
    {
        public string SendMail(string ToEmail,string Token)
        {
            string FromEmail = "pagadalachirudeep@gmail.com";
            MailMessage Message = new MailMessage(FromEmail,ToEmail);
            string MailBody = "Token for Reset Password : " + Token;
            Message.Subject = "Token generate for reset password";
            Message.Body = MailBody.ToString();
            Message.BodyEncoding = Encoding.UTF8;
            Message.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com",587);
            NetworkCredential networkCredential = new NetworkCredential(FromEmail, "lluu rtap efhe hxet");

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = networkCredential;
            smtpClient.Send(Message);
            return ToEmail;

        }
    }
}
