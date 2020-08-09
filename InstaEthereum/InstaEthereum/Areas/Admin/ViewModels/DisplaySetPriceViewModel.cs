using InstaEthereum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaEthereum.Areas.Admin.ViewModels
{
    public class DisplaySetPriceViewModel
    {
        public IEnumerable<SetPrice> SetPrices { get; set; }
        public decimal BinanceETHPrice { get; set; }
        public decimal WazirXETHPrice { get; set; }        
    }
}