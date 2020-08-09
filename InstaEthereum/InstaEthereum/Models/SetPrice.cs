using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InstaEthereum.Models
{
    [Table("aspsetprice")]
    public class SetPrice
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal AddPercent { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
    }
}