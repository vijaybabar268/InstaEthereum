using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InstaEthereum.ViewModels
{
    public class StepOneViewModel
    {
        public decimal SetPrice { get; set; }

        [Required]
        [Display(Name = "Ethereum Qty")]
        public decimal? EthereumQty { get; set; }

        public decimal MinEthBuy { get; set; }

        public decimal MaxEthBuy { get; set; }                
    }
}