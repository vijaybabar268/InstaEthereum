using System.ComponentModel.DataAnnotations;

namespace InstaEthereum.Models
{
    public class UserLoginViewModel
    {
        [Required]
        [Display(Name = "Email")]        
        [EmailAddress]
        public string Email { get; set; }                
    }
}
