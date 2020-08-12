using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstaEthereum.Models
{
    [Table("asporder")]
    public class Order
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public DateTime OrderDateTime { get; set; }
        public decimal EthereumQty { get; set; }
        public decimal PurchasePrice { get; set; }
        public int UserId { get; set; }
        public byte Status { get; set; }    
    }
}