using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InstaEthereum.ViewModels
{
    public class StepOneViewModel
    {
        public decimal EthPrice { get; set; }

        [Required]
        [Display(Name = "ETH")]
        public decimal? EthereumQty { get; set; }

        [Required]
        [Display(Name = "INR")]
        public decimal? EthereumINR { get; set; }

        public decimal MinEthBuy { get; set; }

        public decimal MaxEthBuy { get; set; }                
    }
}