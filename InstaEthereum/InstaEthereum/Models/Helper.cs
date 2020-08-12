using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}