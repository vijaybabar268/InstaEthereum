using System.ComponentModel.DataAnnotations.Schema;

namespace InstaEthereum.Models
{
    [Table("aspnetroles")]
    public class AspNetRole
    {
        public int Id { get; set; }
        public string Name { get; set; }        
    }
}