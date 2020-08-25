using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstaEthereum.Models
{
    [Table("aspnetstocks")]
    public class Stock
    {
        public int Id { get; set; }
        public DateTime? Datetime { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? PurchaseQty { get; set; }
        public decimal? SoldQty { get; set; }
        public decimal? ClosingBalance { get; set; }    
    }
}