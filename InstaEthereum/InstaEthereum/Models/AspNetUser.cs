using System.ComponentModel.DataAnnotations.Schema;

namespace InstaEthereum.Models
{
    [Table("aspnetusers")]
    public class AspNetUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }        
        public string WalletAddress { get; set; }
        public int RoleId { get; set; }
    }
}