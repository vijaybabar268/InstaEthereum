using InstaEthereum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaEthereum.Areas.Admin.ViewModels
{
    public class StockViewModel
    {
        public IEnumerable<Stock> Stocks { get; set; }
        public int Id { get; set; }
        public decimal EthMinPurchaseLimit { get; set; }
        public decimal EthMaxPurchaseLimit { get; set; }
        public decimal? EthereumQty { get; set; }
    }        
}