using System.ComponentModel.DataAnnotations;

namespace InstaEthereum.ViewModels
{
    public class StepFiveViewModel
    {
        public decimal EthPrice { get; set; }

        public decimal MinEthBuy { get; set; }

        public decimal MaxEthBuy { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "UPI Address")]
        public string UPIAddress { get; set; }
    }
}