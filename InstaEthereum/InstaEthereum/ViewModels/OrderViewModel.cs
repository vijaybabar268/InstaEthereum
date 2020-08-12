using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaEthereum.ViewModels
{
    public class OrderViewModel
    {
        public decimal OneEthPrice { get; set; }
        public decimal EthereumQty { get; set; }
        public string WalletAddress { get; set; }
        public decimal PayableAmount { get; set; }
    }
}