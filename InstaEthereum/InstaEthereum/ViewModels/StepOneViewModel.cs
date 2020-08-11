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
        public int? EthereumQty { get; set; }

        public byte MinEthBuy { get; set; }

        public byte MaxEthBuy { get; set; }                
    }
}