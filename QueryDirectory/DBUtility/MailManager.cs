using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace QueryDirectory.DBUtility
{
    class MailManager
    {
        internal bool SendMail(string tomail, string ccmail, string subject, string emailbody)
        {
            bool retVal = true;

            MailMessage _mail = new MailMessage();
            _mail.From = new MailAddress("automail@mutualtrustbank.com");

            _mail.To.Add(tomail);
            if (!ccmail.Equals(""))
            {
                _mail.CC.Add(ccmail);
            }

            _mail.Subject = subject;
            _mail.Body = emailbody;
            _mail.IsBodyHtml = true;

            //SmtpClient client = new SmtpClient("mail.mutualtrustbank.com");
            SmtpClient client = new SmtpClient("10.45.2.41"); 
            client.UseDefaultCredentials = false;

            try
            {
                client.Send(_mail);
                return true;
            }
            catch (Exception ex)
            {
                retVal = false;
            }

            return retVal;
        }
    }
}
