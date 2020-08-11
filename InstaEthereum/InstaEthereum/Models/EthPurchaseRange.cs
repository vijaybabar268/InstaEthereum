using System.ComponentModel.DataAnnotations.Schema;

namespace InstaEthereum.Models
{
    [Table("aspethminmaxrange")]
    public class EthPurchaseRange
    {
        public int Id { get; set; }        
        public decimal Min { get; set; }
        public decimal Max { get; set; }        
    }
}