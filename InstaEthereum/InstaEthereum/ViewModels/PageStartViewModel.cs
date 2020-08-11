using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InstaEthereum.ViewModels
{
    public class PageStartViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public decimal MinEthBuy { get; set; }

        public decimal MaxEthBuy { get; set; }

        public decimal EthPrice { get; set; }   
    }
}