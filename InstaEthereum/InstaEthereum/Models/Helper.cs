using System;
using System.Collections.Generic;
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

        public static bool SendEmail(string to_email)
        {
            bool isEmailSent = false;

            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("vijbab.kalp@gmail.com", "Insta Ethereum");
                message.To.Add(new MailAddress(to_email));
                message.Subject = "Test Email";
                message.IsBodyHtml = true; //to make message body as html  
                var htmlString = "Test Email";
                message.Body = htmlString;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("vijbab.kalp@gmail.com", "vijbab@123#321");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

                isEmailSent = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return isEmailSent;
        }
    }
}