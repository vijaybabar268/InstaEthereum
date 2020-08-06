using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstaEthereum.Models
{
    [Table("aspnetusers")]
    public class AspNetUser
    {
        [Key]
        public int Id { get; set; }        
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string WalletAddress { get; set; }
    }
}