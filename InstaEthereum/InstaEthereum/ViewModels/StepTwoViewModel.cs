using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InstaEthereum.ViewModels
{
    public class StepTwoViewModel
    {
        public decimal SetPrice { get; set; }
                
        public byte MinEthBuy { get; set; }

        public byte MaxEthBuy { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name ="Wallet Address")]
        public string WalletAddress { get; set; }
    }
}