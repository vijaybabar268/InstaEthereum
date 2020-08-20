using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace InstaEthereum.Models
{
    public static class Helper
    {
        private static readonly ApplicationDbContext _context;

        static Helper()
        {
            _context = new ApplicationDbContext();
        }

        public static string parseValueIntoCurrency(decimal number)
        {
            string curCulture = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo(curCulture).NumberFormat;
            currencyFormat.CurrencyNegativePattern = 1;
            var OriginalPrice = number.ToString("c", currencyFormat);

            return OriginalPrice;
        }

        public static AspNetUser GetUserEmail(int id)
        {
            return _context.AspNetUsers.FirstOrDefault(u => u.Id == id);
        }

        public static string SendEmail(string to_email)
        {
            var isEmailSent = "false";
            var liveBaseUrl = "http://instacrypto.io/";
            var localBaseUrl = "https://localhost:44392/";

            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("vijbab.kalp@gmail.com", "Insta Ethereum");
                message.To.Add(new MailAddress(to_email));
                message.Subject = "Insta Ethereum - Payment Link";
                message.IsBodyHtml = true; //to make message body as html  
                var htmlString = @"Dear User,<br/><br/>" +
                                  "Payment Link - <br/>"+ System.Configuration.ConfigurationManager.AppSettings["IsProduction"] == "true" ? liveBaseUrl : localBaseUrl + "BuyEthereum/StepFive?email=" + to_email+"&dt="+ DateTime.Now.ToString("d-M-yyyy|H:m:ss") + "<br/><br/><br/>" +
                                  "Thanks and Regards,<br/>" +
                                  "Team Insta Ethereum";
                message.Body = htmlString;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("vijbab.kalp@gmail.com", "vijbab@123#321");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

                isEmailSent = "true";
            }
            catch (Exception ex)
            {                
                isEmailSent = "Error:"+ ex.Message;
            }

            return isEmailSent;
        }

        public static int GetTransactionCount(int id)
        {
            var getTransactionCount = _context.Orders.Where(c => c.UserId == id).Count();

            return getTransactionCount;
        }
    }
}