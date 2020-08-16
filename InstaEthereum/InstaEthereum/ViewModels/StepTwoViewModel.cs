using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InstaEthereum.ViewModels
{
    public class StepTwoViewModel
    {
        public decimal EthPrice { get; set; }
                
        public decimal MinEthBuy { get; set; }

        public decimal MaxEthBuy { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name ="Wallet Address")]
        public string WalletAddress { get; set; }
    }
}